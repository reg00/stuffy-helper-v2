using EnsureThat;
using StuffyHelper.Authorization.Core.Models.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace StuffyHelper.Authorization.Core.Features.Friend
{
    public class FriendsRequest
    {
        public Guid Id { get; set; }
        public bool IsComfirmed { get; set; }
        public string UserIdFrom { get; set; } = string.Empty;
        public string UserIdTo { get; set; } = string.Empty;

        [ForeignKey("UserIdFrom")]
        public virtual StuffyUser UserFrom { get; set; } = new();
        [ForeignKey("UserIdTo")]
        public virtual StuffyUser UserTo { get; set; } = new();

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
