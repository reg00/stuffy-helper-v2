using EnsureThat;
using StuffyHelper.Authorization.Core.Models.User;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Authorization.Core.Models.Friends
{
    public class AspNetFriends
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string UserAId { get; set; }
        [Required]
        public string UserBId { get; set; }
        [Required]
        public DateTime FriendsSince { get; set; }

        public virtual StuffyUser UserA { get; set; }
        public virtual StuffyUser UserB { get; set; }

        public AspNetFriends(string userAId, string userBId)
        {
            EnsureArg.IsNotNullOrWhiteSpace(userAId, nameof(userAId));
            EnsureArg.IsNotNullOrWhiteSpace(userBId, nameof(userBId));

            UserAId = userAId;
            UserBId = userBId;
            FriendsSince = DateTime.UtcNow;
        }
    }
}
