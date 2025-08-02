using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Messages;

namespace StuffyHelper.ApiGateway.Core.Services.Interfaces;

/// <summary>
/// Friend service
/// </summary>
public interface IFriendService
{
    /// <summary>
    /// Get user friends
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="limit">Pagination limit</param>
    /// <param name="offset">Pagination offset</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<Response<UserShortEntry>> GetFriends(string token, int limit = 20, int offset = 0, CancellationToken cancellationToken = default);
}