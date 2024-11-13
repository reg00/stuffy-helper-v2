using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Messages;

namespace StuffyHelper.Authorization.Contracts.Clients.Interface;

public interface IFriendClient
{
    public Task<Response<UserShortEntry>> GetAsync(
        string token,
        int limit = 20,
        int offset = 0,
        CancellationToken cancellationToken = default);
}