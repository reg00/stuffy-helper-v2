using StuffyHelper.Authorization.Contracts.Models;

namespace StuffyHelper.Authorization.Contracts.Clients.Interface;

public interface IFriendRequestClient
{
    public Task<IReadOnlyList<FriendsRequestShort>> GetSendedRequestsAsync(
        string token,
        CancellationToken cancellationToken = default);

    public Task<IReadOnlyList<FriendsRequestShort>> GetIncomingRequestsAsync(
        string token,
        CancellationToken cancellationToken = default);

    public Task<FriendsRequestShort> GetAsync(
        string token,
        Guid requestId,
        CancellationToken cancellationToken = default);

    public Task ConfirmAsync(
        string token,
        Guid requestId,
        CancellationToken cancellationToken = default);

    public Task<FriendsRequestShort> AddRequestAsync(
        string token,
        string userId,
        CancellationToken cancellationToken = default);

    public Task DeleteRequestAsync(
        string token,
        Guid requestId,
        CancellationToken cancellationToken = default);
}