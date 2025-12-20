using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Messages;

namespace StuffyHelper.Authorization.Contracts.Clients.Interface;

/// <summary>
/// Interface for work with friends
/// </summary>
public interface IFriendClient
{
    /// <summary>
    /// Get user friends
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="limit">Pagination limit</param>
    /// <param name="offset">Pagination offset</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task<Response<UserShortEntry>> GetAsync(
        string token,
        int limit = 20,
        int offset = 0,
        CancellationToken cancellationToken = default);
}