using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Common.Messages;

namespace StuffyHelper.Authorization.Data.Repository.Interfaces;

/// <summary>
/// Interface for work with friends in our database
/// </summary>
public interface IFriendRepository
{
    /// <summary>
    /// Add friend for user in our database
    /// </summary>
    /// <param name="friendEntry">Friend entry</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    Task<FriendEntry> AddFriendAsync(FriendEntry friendEntry, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete friend from our database
    /// </summary>
    /// <param name="friendId">Friend id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    Task DeleteFriendAsync(Guid friendId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="limit"></param>
    /// <param name="offset"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AuthResponse<FriendEntry>> GetFriends(string userId, int limit = 20, int offset = 0, CancellationToken cancellationToken = default);
}