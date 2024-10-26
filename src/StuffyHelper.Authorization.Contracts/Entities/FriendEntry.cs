using System.ComponentModel.DataAnnotations;
using EnsureThat;

namespace StuffyHelper.Authorization.Contracts.Entities;

public class FriendEntry
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string UserId { get; set; }
    [Required]
    public string FriendId { get; set; }
    [Required]
    public DateTime FriendsSince { get; set; }

    public virtual StuffyUser User { get; set; }
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