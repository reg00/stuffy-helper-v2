using System.Security.Claims;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Messages;

namespace StuffyHelper.Authorization.Core.Services.Interfaces;

/// <summary>
/// Friend service
/// </summary>
public interface IFriendService
{
    /// <summary>
    /// Add friend for user in our database
    /// </summary>
    /// <param name="userId">User id</param>
    /// <param name="friendId">Friend Id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<FriendShortEntry> AddFriendAsync(
        string userId,
        string friendId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete friend from our database
    /// </summary>
    /// <param name="friendshipId">Friendship id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task DeleteFriendAsync(Guid friendshipId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get user friends
    /// </summary>
    /// <param name="user">User claims</param>
    /// <param name="limit">Pagination limit</param>
    /// <param name="offset">Pagination offset</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<AuthResponse<UserShortEntry>> GetFriends(ClaimsPrincipal user, int limit = 20, int offset = 0, CancellationToken cancellationToken = default);
}