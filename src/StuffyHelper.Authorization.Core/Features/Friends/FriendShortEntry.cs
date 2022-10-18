using EnsureThat;
using StuffyHelper.Authorization.Core.Features.Friends;

namespace StuffyHelper.Authorization.Core.Features.Friend
{
    public class FriendShortEntry
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string FriendId { get; set; }
        public string UserName { get; set; }
        public string FriendName { get; set; }

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
