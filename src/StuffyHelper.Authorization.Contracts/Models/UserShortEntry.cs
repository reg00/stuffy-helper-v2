namespace StuffyHelper.Authorization.Contracts.Models;

/// <summary>
/// Record for short user entry
/// </summary>
public class UserShortEntry
{
    /// <summary>
    /// Identifier
    /// </summary>
    public string Id { get; init; } = string.Empty;
    
    /// <summary>
    /// Username
    /// </summary>
    public string Name { get; init; } = string.Empty;
    
    /// <summary>
    /// user avatar image url
    /// </summary>
    public Uri? ImageUri { get; init; }
}