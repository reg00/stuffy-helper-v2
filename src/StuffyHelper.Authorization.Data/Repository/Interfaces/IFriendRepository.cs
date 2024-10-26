using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Common.Messages;

namespace StuffyHelper.Authorization.Data.Repository.Interfaces;

public interface IFriendRepository
{
    Task<FriendEntry> AddFriendAsync(FriendEntry friendEntry, CancellationToken cancellationToken = default);

    Task DeleteFriendAsync(Guid friendshipId, CancellationToken cancellationToken = default);

    Task<AuthResponse<FriendEntry>> GetFriends(string userId, int limit = 20, int offset = 0, CancellationToken cancellationToken = default);
}