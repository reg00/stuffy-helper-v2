using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Authorization.Contracts.Entities;

/// <summary>
/// Record for work with friends
/// </summary>
public class FriendEntry
{
    /// <summary>
    /// Identifier
    /// </summary>
    [Required]
    public Guid Id { get; init; }
    
    /// <summary>
    /// User id
    /// </summary>
    [Required]
    public string UserId { get; init; }
    
    /// <summary>
    /// Friend id
    /// </summary>
    [Required]
    public string FriendId { get; init; }
    
    /// <summary>
    /// Date when user became friend
    /// </summary>
    [Required]
    public DateTime FriendsSince { get; init; }

    /// <summary>
    /// Linked user entity
    /// </summary>
    public StuffyUser User { get; init; }
    
    /// <summary>
    /// Linked friend user entity
    /// </summary>
    public StuffyUser Friend { get; init; }
}