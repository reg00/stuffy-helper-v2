using StuffyHelper.Authorization.Core.Features.Friend;
using StuffyHelper.Authorization.Core.Models;
using StuffyHelper.Authorization.Core.Models.User;
using System.Security.Claims;

namespace StuffyHelper.Authorization.Core.Features.Friends
{
    public interface IFriendService
    {
        Task<FriendShortEntry> AddFriendAsync(
            string userId,
            string friendId,
            CancellationToken cancellationToken = default);

        Task DeleteFriendAsync(Guid friendshipId, CancellationToken cancellationToken = default);

        Task<AuthResponse<UserShortEntry>> GetFriends(ClaimsPrincipal user, int limit = 20, int offset = 0, CancellationToken cancellationToken = default);
    }
}
