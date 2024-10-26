using EnsureThat;
using Microsoft.AspNetCore.Identity;
using StuffyHelper.Authorization.Core1.Features.Avatar;
using StuffyHelper.Authorization.Core1.Features.Friend;
using StuffyHelper.Authorization.Core1.Features.Friends;

namespace StuffyHelper.Authorization.Core1.Models.User
{
    public class StuffyUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public Uri? ImageUri { get; set; }

        public virtual List<FriendsRequest> IncomingRequests { get; set; } = new List<FriendsRequest>();
        public virtual List<FriendsRequest> SendedRequests { get; set; } = new List<FriendsRequest>();
        public virtual List<FriendEntry> Friends { get; set; } = new List<FriendEntry>();
        public virtual AvatarEntry? Avatar { get; set; }

        public void PatchFrom(UpdateModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            FirstName = model.FirstName;
            MiddleName = model.MiddleName;
            LastName = model.LastName;
            UserName = model.Username;
            PhoneNumber = model.Phone;
        }
    }
}
