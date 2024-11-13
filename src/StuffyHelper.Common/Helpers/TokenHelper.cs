using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
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
        return new StuffyClaims()
        {
            UserId = claims.First(c => c.Type == ClaimTypes.Sid).Value,
            Username = claims.First(c => c.Type == ClaimTypes.Name).Value,
            Roles = claims.Where(c => c.Type == ClaimTypes.Role).Select(x => x.Value).ToList()
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
}