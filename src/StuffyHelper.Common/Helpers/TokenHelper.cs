using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using StuffyHelper.Common.Contracts;

namespace StuffyHelper.Common.Helpers;

/// <summary>
/// Helper for work with token
/// </summary>
public static class TokenHelper
{
    /// <summary>
    /// Retrurn user claims from token
    /// </summary>
    /// <param name="token">JWT token</param>
    public static StuffyClaims GetUserClaims(this string token)
    {
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token); 
        
        var claims = jwtToken.Claims.ToList();

        return new StuffyClaims()
        {
            UserId = claims.First(c => c.Type == ClaimTypes.Sid).Value,
            Username = claims.First(c => c.Type == ClaimTypes.Name).Value,
        };
    }
}