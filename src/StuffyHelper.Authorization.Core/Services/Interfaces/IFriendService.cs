using System.Security.Claims;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Messages;

namespace StuffyHelper.Authorization.Core.Services.Interfaces;

public interface IFriendService
{
    Task<FriendShortEntry> AddFriendAsync(
        string userId,
        string friendId,
        CancellationToken cancellationToken = default);

    Task DeleteFriendAsync(Guid friendshipId, CancellationToken cancellationToken = default);

    Task<AuthResponse<UserShortEntry>> GetFriends(ClaimsPrincipal user, int limit = 20, int offset = 0, CancellationToken cancellationToken = default);
}