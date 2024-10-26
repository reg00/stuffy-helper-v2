using System.Security.Claims;
using StuffyHelper.Authorization.Core1.Features.Friend;
using StuffyHelper.Authorization.Core1.Models;
using StuffyHelper.Authorization.Core1.Models.User;

namespace StuffyHelper.Authorization.Core1.Features.Friends
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
