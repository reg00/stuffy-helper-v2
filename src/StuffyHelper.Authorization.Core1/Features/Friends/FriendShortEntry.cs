using EnsureThat;
using StuffyHelper.Authorization.Core1.Features.Friends;

namespace StuffyHelper.Authorization.Core1.Features.Friend
{
    public class FriendShortEntry
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string FriendId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string FriendName { get; set; } = string.Empty;

        public FriendShortEntry(FriendEntry friendship)
        {
            EnsureArg.IsNotNull(friendship, nameof(friendship));

            Id = friendship.Id;
            UserId = friendship.UserId;
            FriendId = friendship.FriendId;
            UserName = friendship.User.UserName;
            UserName = friendship.Friend.UserName;
        }
    }
}
