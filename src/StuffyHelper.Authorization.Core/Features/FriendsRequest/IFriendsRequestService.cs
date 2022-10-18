using System.Security.Claims;

namespace StuffyHelper.Authorization.Core.Features.FriendsRequest
{
    public interface IFriendsRequestService
    {
        Task<FriendsRequestShort> GetRequestAsync(Guid requestId, CancellationToken cancellationToken);

        Task<IEnumerable<FriendsRequestShort>> GetSendedRequestsAsync(
           ClaimsPrincipal user,
           CancellationToken cancellationToken = default);

        Task<IEnumerable<FriendsRequestShort>> GetIncomingRequestsAsync(
            ClaimsPrincipal user,
            CancellationToken cancellationToken = default);

        Task<FriendsRequestShort> AddRequestAsync(
            ClaimsPrincipal icomingUser,
            string requestUserId,
            CancellationToken cancellationToken = default);

        Task DeleteRequestAsync(Guid requestId, CancellationToken cancellationToken = default);
    }
}
