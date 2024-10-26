using StuffyHelper.Authorization.Core1.Models;

namespace StuffyHelper.Authorization.Core1.Features.Friends
{
    public interface IFriendStore
    {
        Task<FriendEntry> AddFriendAsync(FriendEntry friendEntry, CancellationToken cancellationToken = default);

        Task DeleteFriendAsync(Guid friendshipId, CancellationToken cancellationToken = default);

        Task<AuthResponse<FriendEntry>> GetFriends(string userId, int limit = 20, int offset = 0, CancellationToken cancellationToken = default);
    }
}
