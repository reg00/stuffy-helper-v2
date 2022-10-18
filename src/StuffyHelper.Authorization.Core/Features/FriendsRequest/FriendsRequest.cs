using EnsureThat;
using StuffyHelper.Authorization.Core.Models.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace StuffyHelper.Authorization.Core.Features.Friend
{
    public class FriendsRequest
    {
        public Guid Id { get; set; }
        public bool IsComfirmed { get; set; }
        public string UserIdFrom { get; set; }
        public string UserIdTo { get; set; }

        [ForeignKey("UserIdFrom")]
        public virtual StuffyUser UserFrom { get; set; }
        [ForeignKey("UserIdTo")]
        public virtual StuffyUser UserTo { get; set; }

        public FriendsRequest(string userIdFrom, string userIdTo)
        {
            EnsureArg.IsNotNullOrWhiteSpace(userIdFrom, nameof(userIdFrom));
            EnsureArg.IsNotNullOrWhiteSpace(userIdTo, nameof(userIdTo));

            UserIdFrom = userIdFrom;
            UserIdTo = userIdTo;
            IsComfirmed = false;
        }
    }
}
