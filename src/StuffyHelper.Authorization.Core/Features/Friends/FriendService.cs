using EnsureThat;
using StuffyHelper.Authorization.Core.Exceptions;
using StuffyHelper.Authorization.Core.Features.Friend;
using StuffyHelper.Authorization.Core.Models;
using StuffyHelper.Authorization.Core.Models.User;
using System.Security.Claims;

namespace StuffyHelper.Authorization.Core.Features.Friends
{
    public class FriendService : IFriendService
    {
        private readonly IFriendStore _friendshipStore;
        private readonly IAuthorizationService _authorizationService;

        public FriendService(IFriendStore friendshipStore, IAuthorizationService authorizationService)
        {
            _friendshipStore = friendshipStore;
            _authorizationService = authorizationService;
        }

        public async Task<FriendShortEntry> AddFriendAsync(
            string userId,
            string friendId,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNullOrWhiteSpace(userId, nameof(userId));
            EnsureArg.IsNotNullOrWhiteSpace(friendId, nameof(friendId));

            var stuffyUser = await _authorizationService.GetUser(userId: userId);
            var friend = await _authorizationService.GetUser(userId: friendId);

            if (stuffyUser.Id == friend.Id)
                throw new AuthorizationException("Can not request yourself.");

            var request = new FriendEntry(stuffyUser.Id, friend.Id);

            var result = await _friendshipStore.AddFriendAsync(request, cancellationToken);
            return new FriendShortEntry(result);
        }

        public async Task DeleteFriendAsync(Guid friendshipId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(friendshipId, nameof(friendshipId));

            await _friendshipStore.DeleteFriendAsync(friendshipId, cancellationToken);
        }

        public async Task<AuthResponse<UserShortEntry>> GetFriends(ClaimsPrincipal user, int limit = 20, int offset = 0, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(user, nameof(user));

            var userName = user?.Identity?.Name;

            if (userName == null)
                throw new AuthorizationException("Authorization error");

            var stuffyUser = await _authorizationService.GetUser(userName);
            var response = await _friendshipStore.GetFriends(stuffyUser.Id, limit, offset, cancellationToken);

            return new AuthResponse<UserShortEntry>()
            {
                Data = response.Data.Select(x => new UserShortEntry(x.Friend)),
                Total = response.Total,
                TotalPages = response.TotalPages
            };
        }
    }
}
