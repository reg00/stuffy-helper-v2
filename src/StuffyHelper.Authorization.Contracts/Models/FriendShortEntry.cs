namespace StuffyHelper.Authorization.Contracts.Models;

/// <summary>
/// Record for short friend entry
/// </summary>
public class FriendShortEntry
{
    /// <summary>
    /// Identifier
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// User id
    /// </summary>
    public string UserId { get; init; } = string.Empty;
    
    /// <summary>
    /// Firend id
    /// </summary>
    public string FriendId { get; init; } = string.Empty;
    
    /// <summary>
    /// User name
    /// </summary>
    public string? UserName { get; init; } = string.Empty;
    
    /// <summary>
    /// Friend name
    /// </summary>
    public string? FriendName { get; init; } = string.Empty;
}