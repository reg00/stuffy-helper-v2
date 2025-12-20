using StuffyHelper.Authorization.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services.Interfaces;

/// <summary>
/// Friend request service
/// </summary>
public interface IFriendsRequestService
{
    /// <summary>
    /// Confirm friend request in our database by id
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="requestId">Friend request id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task ConfirmRequest(string token, Guid requestId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get friend request by its id
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="requestId">Friend request id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<FriendsRequestShort> GetRequestAsync(string token, Guid requestId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Get all sent friend requests by user id
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IReadOnlyList<FriendsRequestShort>> GetSentRequestsAsync(string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all incoming friend requests by user id
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IReadOnlyList<FriendsRequestShort>> GetIncomingRequestsAsync(string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add friend request in our database
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="userId">Friend id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<FriendsRequestShort> AddRequestAsync(string token, string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete friend request from our database by id
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="requestId">Friend request id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task DeleteRequestAsync(string token, Guid requestId, CancellationToken cancellationToken = default);
}