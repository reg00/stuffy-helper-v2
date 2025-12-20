namespace StuffyHelper.Common.Contracts;

/// <summary>
/// User claims from token
/// </summary>
public record StuffyClaims
{
    /// <summary>
    /// User id from token
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// user name from token
    /// </summary>
    public string Username { get; init; } = string.Empty;
    
    /// <summary>
    /// User roles
    /// </summary>
    public List<string> Roles { get; init; } = new();
    
    /// <summary>
    /// user avatar image url
    /// </summary>
    public Uri? ImageUri { get; init; }
}