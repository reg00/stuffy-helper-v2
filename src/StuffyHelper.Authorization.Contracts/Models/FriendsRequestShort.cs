namespace StuffyHelper.Authorization.Contracts.Models;

/// <summary>
/// Record for short friend request entry
/// </summary>
public class FriendsRequestShort
{
    /// <summary>
    /// Identifier
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// User id from
    /// </summary>
    public string UserIdFrom { get; init; }
    
    /// <summary>
    /// User id to
    /// </summary>
    public string UserIdTo { get; init; } = string.Empty;
    
    /// <summary>
    /// Username from
    /// </summary>
    public string? UserNameFrom { get; init; } = string.Empty;
    
    /// <summary>
    /// Username to
    /// </summary>
    public string? UserNameTo { get; init; } = string.Empty;
}