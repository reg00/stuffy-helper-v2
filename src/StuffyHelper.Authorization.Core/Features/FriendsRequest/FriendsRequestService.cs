using EnsureThat;
using StuffyHelper.Authorization.Core.Exceptions;
using System.Security.Claims;

namespace StuffyHelper.Authorization.Core.Features.FriendsRequest
{
    public class FriendsRequestService : IFriendsRequestService
    {
        private readonly IFriendsRequestStore _requestStore;
        private readonly IAuthorizationService _authorizationService;

        public FriendsRequestService(
            IFriendsRequestStore requestStore,
            IAuthorizationService authorizationService)
        {
            _requestStore = requestStore;
            _authorizationService = authorizationService;
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

            var stuffyUser = await _authorizationService.GetUser(user.Identity.Name);
            var resp = await _requestStore.GetSendedRequestsAsync(stuffyUser.Id, cancellationToken);
            var requests = new List<FriendsRequestShort>();

            return resp.Select(x => new FriendsRequestShort(x));
        }

        public async Task<IEnumerable<FriendsRequestShort>> GetIncomingRequestsAsync(
            ClaimsPrincipal user,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(user, nameof(user));

            var stuffyUser = await _authorizationService.GetUser(user.Identity.Name);
            var resp = await _requestStore.GetIncomingRequestsAsync(stuffyUser.Id, cancellationToken);
            var requests = new List<FriendsRequestShort>();

            return resp.Select(x => new FriendsRequestShort(x));
        }

        public async Task<FriendsRequestShort> AddRequestAsync(
            ClaimsPrincipal user,
            string userId,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNullOrWhiteSpace(userId, nameof(userId));
            EnsureArg.IsNotNull(user, nameof(user));

            var incomingUser = await _authorizationService.GetUser(user?.Identity?.Name);
            var requestUser = await _authorizationService.GetUser(userId: userId);

            if (incomingUser.Id == requestUser.Id)
                throw new AuthorizationException("Can not request yourself.");

            var request = new FriendsRequest(incomingUser?.Id, requestUser.Id);

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
