using System.Security.Claims;
using StuffyHelper.Authorization.Contracts.Models;

namespace StuffyHelper.Authorization.Core.Services.Interfaces;

/// <summary>
/// Friend request service
/// </summary>
public interface IFriendsRequestService
{
    /// <summary>
    /// Confirm friend request in our database by id
    /// </summary>
    /// <param name="requestId">Friend request id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task ConfirmRequest(Guid requestId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get friend request by its id
    /// </summary>
    /// <param name="requestId">Frined request id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<FriendsRequestShort> GetRequestAsync(Guid requestId, CancellationToken cancellationToken);

    /// <summary>
    /// Get all sended friend requests by user id
    /// </summary>
    /// <param name="user">User claims</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IReadOnlyList<FriendsRequestShort>> GetSendedRequestsAsync(
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all incoming friend requests by user id
    /// </summary>
    /// <param name="user">User claims</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IReadOnlyList<FriendsRequestShort>> GetIncomingRequestsAsync(
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Add friend request in our database
    /// </summary>
    /// <param name="friend">Friend user</param>
    /// <param name="requestUserId">Friend request id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<FriendsRequestShort> AddRequestAsync(
        ClaimsPrincipal friend,
        string requestUserId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete friend request from our database by id
    /// </summary>
    /// <param name="requestId">Friend request id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task DeleteRequestAsync(Guid requestId, CancellationToken cancellationToken = default);
}