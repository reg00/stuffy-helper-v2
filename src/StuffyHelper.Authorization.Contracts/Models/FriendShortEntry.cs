using EnsureThat;
using StuffyHelper.Authorization.Contracts.Entities;

namespace StuffyHelper.Authorization.Contracts.Models;

public class FriendShortEntry
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string FriendId { get; set; } = string.Empty;
    public string? UserName { get; set; } = string.Empty;
    public string? FriendName { get; set; } = string.Empty;

    public FriendShortEntry(FriendEntry friendship)
    {
        EnsureArg.IsNotNull(friendship, nameof(friendship));

        Id = friendship.Id;
        UserId = friendship.UserId;
        FriendId = friendship.FriendId;
        UserName = friendship.User.UserName;
        FriendName = friendship.Friend.UserName;
    }
}