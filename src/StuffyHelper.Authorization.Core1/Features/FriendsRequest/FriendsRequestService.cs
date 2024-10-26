using EnsureThat;
using Reg00.Infrastructure.Errors;
using System.Security.Claims;
using StuffyHelper.Authorization.Core1.Exceptions;
using StuffyHelper.Authorization.Core1.Features.Friends;

namespace StuffyHelper.Authorization.Core1.Features.Friend
{
    public class FriendsRequestService : IFriendsRequestService
    {
        private readonly IFriendsRequestStore _requestStore;
        private readonly IAuthorizationService _authorizationService;
        private readonly IFriendService _friendshipService;

        public FriendsRequestService(
            IFriendsRequestStore requestStore,
            IAuthorizationService authorizationService,
            IFriendService friendshipService)
        {
            _requestStore = requestStore;
            _authorizationService = authorizationService;
            _friendshipService = friendshipService;
        }

        public async Task AcceptRequest(Guid requestId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(requestId, nameof(requestId));

            var request = await _requestStore.GetRequest(requestId, cancellationToken);
            await _friendshipService.AddFriendAsync(request.UserIdFrom, request.UserIdTo, cancellationToken);
            await _friendshipService.AddFriendAsync(request.UserIdTo, request.UserIdFrom, cancellationToken);
            await _requestStore.ComfirmRequestAsync(requestId, cancellationToken);
        }

        public async Task<FriendsRequestShort> GetRequestAsync(Guid requestId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(requestId, nameof(requestId));

            var entry = await _requestStore.GetRequest(requestId, cancellationToken);
            return new FriendsRequestShort(entry);
        }

        public async Task<IEnumerable<FriendsRequestShort>> GetSendedRequestsAsync(
           ClaimsPrincipal user,
           CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(user, nameof(user));

            var userName = user?.Identity?.Name;

            if (userName == null)
                throw new ForbiddenException("Authorization error");

            var stuffyUser = await _authorizationService.GetUserByName(userName);
            var resp = await _requestStore.GetSendedRequestsAsync(stuffyUser.Id, cancellationToken);

            return resp.Select(x => new FriendsRequestShort(x));
        }

        public async Task<IEnumerable<FriendsRequestShort>> GetIncomingRequestsAsync(
            ClaimsPrincipal user,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(user, nameof(user));

            var userName = user?.Identity?.Name;

            if (userName == null)
                throw new ForbiddenException("Authorization error");

            var stuffyUser = await _authorizationService.GetUserByName(userName);
            var resp = await _requestStore.GetIncomingRequestsAsync(stuffyUser.Id, cancellationToken);

            return resp.Select(x => new FriendsRequestShort(x));
        }

        public async Task<FriendsRequestShort> AddRequestAsync(
            ClaimsPrincipal user,
            string userId,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNullOrWhiteSpace(userId, nameof(userId));
            EnsureArg.IsNotNull(user, nameof(user));

            var userName = user?.Identity?.Name;

            if (userName == null)
                throw new ForbiddenException("Authorization error");

            var incomingUser = await _authorizationService.GetUserByName(userName);
            var requestUser = await _authorizationService.GetUserById(userId);

            if (incomingUser.Id == requestUser.Id)
                throw new AuthorizationException("Can not request yourself.");

            var request = new FriendsRequest(incomingUser.Id, requestUser.Id);

            var result = await _requestStore.AddRequestAsync(request, cancellationToken);
            return new FriendsRequestShort(result);
        }

        public async Task DeleteRequestAsync(Guid requestId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(requestId, nameof(requestId));

            await _requestStore.DeleteRequestAsync(requestId, cancellationToken);
        }
    }
}
