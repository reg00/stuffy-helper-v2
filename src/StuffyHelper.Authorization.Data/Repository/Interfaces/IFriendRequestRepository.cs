using StuffyHelper.Authorization.Contracts.Entities;

namespace StuffyHelper.Authorization.Data.Repository.Interfaces;

/// <summary>
/// Interface for work with friend requests in our database
/// </summary>
public interface IFriendRequestRepository
{
    /// <summary>
    /// Get friend request by its id
    /// </summary>
    /// <param name="requestId">Friend request id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<FriendsRequest> GetRequest(Guid requestId, CancellationToken cancellationToken);

    /// <summary>
    /// Get friend request by userId and FriendId
    /// </summary>
    /// <param name="userId">User id</param>
    /// <param name="friendId">Friend id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    Task<FriendsRequest?> GetRequest(string userId, string friendId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get all sended friend requests by user id
    /// </summary>
    /// <param name="userId">User id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IEnumerable<FriendsRequest>> GetSendedRequestsAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all incoming friend requests by user id
    /// </summary>
    /// <param name="userId">User id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IEnumerable<FriendsRequest>> GetIncomingRequestsAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add friend request in our database
    /// </summary>
    /// <param name="request">Friend request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<FriendsRequest> AddRequestAsync(FriendsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete friend request from our database by id
    /// </summary>
    /// <param name="requestId">Friend request id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task DeleteRequestAsync(Guid requestId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Confirm friend request in our database by id
    /// </summary>
    /// <param name="requestId">Friend request id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task ComfirmRequestAsync(Guid requestId, CancellationToken cancellationToken = default);
}