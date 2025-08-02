using StuffyHelper.Authorization.Contracts.Models;

namespace StuffyHelper.Authorization.Contracts.Clients.Interface;

/// <summary>
/// Friend request client
/// </summary>
public interface IFriendRequestClient
{
    /// <summary>
    /// Get all sent friend requests by user id
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task<IReadOnlyList<FriendsRequestShort>> GetSentRequestsAsync(
        string token,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all incoming friend requests by user id
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task<IReadOnlyList<FriendsRequestShort>> GetIncomingRequestsAsync(
        string token,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get friend request by its id
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="requestId">Friend request id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task<FriendsRequestShort> GetAsync(
        string token,
        Guid requestId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Confirm friend request in our database by id
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="requestId">Friend request id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task ConfirmAsync(
        string token,
        Guid requestId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Add friend request in our database
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="userId">Friend id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task<FriendsRequestShort> AddRequestAsync(
        string token,
        string userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete friend request from our database by id
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="requestId">Friend request id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task DeleteRequestAsync(
        string token,
        Guid requestId,
        CancellationToken cancellationToken = default);
}