using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using StuffyHelper.Common.Configurations;
using StuffyHelper.Common.Contracts;

namespace StuffyHelper.Common.Helpers;

/// <summary>
/// Helper for work with token
/// </summary>
public static class TokenHelper
{
    private static readonly Regex BearerTokenRegex = new(@"Bearer\s+([^\s]+)");
    
    /// <summary>
    /// Retrurn user claims from token
    /// </summary>
    /// <param name="token">JWT token</param>
    public static StuffyClaims GetUserClaims(this string token)
    {
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token); 
        
        return GetClaimsData(jwtToken.Claims.ToList());
    }
    
    /// <summary>
    /// Retrurn user claims from identity
    /// </summary>
    /// <param name="identity">Identity</param>
    public static StuffyClaims GetUserClaims(this ClaimsIdentity? identity)
    {
        if (identity == null)
            throw new ArgumentNullException("Identity not found");
        
        return GetClaimsData(identity.Claims.ToList());
    }

    private static StuffyClaims GetClaimsData(IList<Claim> claims)
    {
        var imageUri = claims.FirstOrDefault(c => c.Type == ClaimTypes.Uri)?.Value;
        
        return new StuffyClaims()
        {
            UserId = claims.First(c => c.Type == ClaimTypes.Sid).Value,
            Username = claims.First(c => c.Type == ClaimTypes.Name).Value,
            Roles = claims.Where(c => c.Type == ClaimTypes.Role).Select(x => x.Value).ToList(),
            ImageUri = string.IsNullOrWhiteSpace(imageUri) ? new Uri(imageUri!) : new Uri("about:blank")
        };
    }

    public static bool TryGetToken(this HttpRequest request, [MaybeNullWhen(false)] out string token)
    {
        token = null;

        if (!request.Headers.TryGetValue(HeaderNames.Authorization, out var headerValue))
            return false;

        var headerToken = headerValue.ToString();

        if (string.IsNullOrWhiteSpace(headerToken))
            return false;

        var match = BearerTokenRegex.Match(headerToken);

        if (!match.Success)
            return false;

        token = match.Groups[1].Value;

        return true;
    }
    
    /// <summary>
    /// Generate system token
    /// </summary>
    public static string GenerateSystemToken(AuthorizationConfiguration config)
    {
        var authClaims = new List<Claim>
        {
            new (ClaimTypes.Sid, Guid.Empty.ToString()),
            new(ClaimTypes.Name, "admin"),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Role, "admin")
        };

        var token = new JwtSecurityToken(
            issuer: config.JWT.ValidIssuer,
            audience: config.JWT.ValidAudience,
            expires: DateTime.UtcNow.AddHours(config.JWT.TokenExpireInHours),
            claims: authClaims,
            signingCredentials: new SigningCredentials(config.JWT.GetSecurityKey(), SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}