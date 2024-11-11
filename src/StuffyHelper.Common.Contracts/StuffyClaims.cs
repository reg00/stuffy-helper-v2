namespace StuffyHelper.Common.Contracts;

/// <summary>
/// User claims from token
/// </summary>
public record StuffyClaims
{
    /// <summary>
    /// User id from token
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// user name from token
    /// </summary>
    public string Username { get; init; } = string.Empty;
}