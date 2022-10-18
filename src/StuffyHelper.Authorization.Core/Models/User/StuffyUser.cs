using EnsureThat;
using Microsoft.AspNetCore.Identity;
using StuffyHelper.Authorization.Core.Features.FriendsRequest;

namespace StuffyHelper.Authorization.Core.Models.User
{
    public class StuffyUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }

        public virtual List<FriendsRequest> IncomingRequests { get; set; } = new List<FriendsRequest>();
        public virtual List<FriendsRequest> SendedRequests { get; set; } = new List<FriendsRequest>();

        public void PatchFrom(UpdateModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            FirstName = model.FirstName;
            MiddleName = model.MiddleName;
            LastName = model.LastName;
            UserName = model.Username;
            Email = model.Email;
            PhoneNumber = model.Phone;
        }
    }
}
