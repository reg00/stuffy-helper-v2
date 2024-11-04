using System.ComponentModel.DataAnnotations;
using EnsureThat;

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
    public Guid Id { get; set; }
    
    /// <summary>
    /// User id
    /// </summary>
    [Required]
    public string UserId { get; set; }
    
    /// <summary>
    /// Friend id
    /// </summary>
    [Required]
    public string FriendId { get; set; }
    
    /// <summary>
    /// Date when user became friend
    /// </summary>
    [Required]
    public DateTime FriendsSince { get; set; }

    /// <summary>
    /// Linked user entity
    /// </summary>
    public virtual StuffyUser User { get; set; }
    
    /// <summary>
    /// Linked friend user entity
    /// </summary>
    public virtual StuffyUser Friend { get; init; }

    public FriendEntry(string userId, string friendId)
    {
        EnsureArg.IsNotNullOrWhiteSpace(userId, nameof(userId));
        EnsureArg.IsNotNullOrWhiteSpace(friendId, nameof(friendId));

        UserId = userId;
        FriendId = friendId;
        FriendsSince = DateTime.UtcNow;
    }
}