using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace StuffyHelper.Common.Configurations;

/// <summary>
/// Authorization configuration
/// </summary>
public class AuthorizationConfiguration
{
    /// <summary>
    /// Default section
    /// </summary>
    public const string DefaultSectionName = "Authorization";
    
    /// <summary>
    /// Database connection string
    /// </summary>
    public string ConnectionString { get; init; } = string.Empty;

    /// <summary>
    /// Jwt token configuration
    /// </summary>
    public JWTOptions JWT { get; init; } = new();
}

/// <summary>
/// Jwt token configuration
/// </summary>
public class JWTOptions
{
    /// <summary>
    /// Audience
    /// </summary>
    public string ValidAudience { get; init; } = string.Empty;
    
    /// <summary>
    /// Issuer
    /// </summary>
    public string ValidIssuer { get; init; } = string.Empty;
    
    /// <summary>
    /// Secret
    /// </summary>
    public string Secret { get; init; } = string.Empty;
    
    /// <summary>
    /// Token expires
    /// </summary>
    public int TokenExpireInHours { get; init; }
    
    /// <summary>
    /// Security key
    /// </summary>
    /// <returns></returns>
    public SymmetricSecurityKey GetSecurityKey() => new(Encoding.UTF8.GetBytes(Secret));
}