using EnsureThat;
using Microsoft.AspNetCore.Identity;
using StuffyHelper.Authorization.Core.Features.Friends;
using StuffyHelper.Authorization.Core.Features.Friend;
using StuffyHelper.Authorization.Core.Features.Avatar;

namespace StuffyHelper.Authorization.Core.Models.User
{
    public class StuffyUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
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
