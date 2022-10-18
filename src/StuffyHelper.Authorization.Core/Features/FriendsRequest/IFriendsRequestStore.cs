namespace StuffyHelper.Authorization.Core.Features.FriendsRequest
{
    public interface IFriendsRequestStore
    {
        Task<FriendsRequest> GetRequest(Guid requestId, CancellationToken cancellationToken);

        Task<IEnumerable<FriendsRequest>> GetSendedRequestsAsync(string userId, CancellationToken cancellationToken = default);

        Task<IEnumerable<FriendsRequest>> GetIncomingRequestsAsync(string userId, CancellationToken cancellationToken = default);

        Task<FriendsRequest> AddRequestAsync(FriendsRequest request, CancellationToken cancellationToken = default);

        Task DeleteRequestAsync(Guid requestId, CancellationToken cancellationToken = default);
    }
}
