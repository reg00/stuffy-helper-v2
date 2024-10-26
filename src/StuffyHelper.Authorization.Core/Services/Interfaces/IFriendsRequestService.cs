using System.Security.Claims;
using StuffyHelper.Authorization.Contracts.Models;

namespace StuffyHelper.Authorization.Core.Services.Interfaces;

public interface IFriendsRequestService
{
    Task AcceptRequest(Guid requestId, CancellationToken cancellationToken = default);

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