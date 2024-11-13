using RestSharp;
using StuffyHelper.Authorization.Contracts.Clients.Interface;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Client;
using StuffyHelper.Common.Web;

namespace StuffyHelper.Authorization.Contracts.Clients;

public class FriendRequestClient : ApiClientBase, IFriendRequestClient
{
    public FriendRequestClient(string baseUrl) : base(baseUrl)
    {
        
    }

    public Task<IReadOnlyList<FriendsRequestShort>> GetSendedRequestsAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.GetSendedRequestsRoute)
            .AddBearerToken(token);

        return Get<IReadOnlyList<FriendsRequestShort>>(request, cancellationToken);
    }
    
    public Task<IReadOnlyList<FriendsRequestShort>> GetIncomingRequestsAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.GetIncomingRequestsRoute)
            .AddBearerToken(token);

        return Get<IReadOnlyList<FriendsRequestShort>>(request, cancellationToken);
    }
    
    public Task<FriendsRequestShort> GetAsync(
        string token,
        Guid requestId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{KnownRoutes.RequestRoute}/{requestId}")
            .AddBearerToken(token);
        
        return Get<FriendsRequestShort>(request, cancellationToken);
    }
    
    public Task ConfirmAsync(
        string token,
        Guid requestId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{KnownRoutes.RequestRoute}/{requestId}/accept")
            .AddBearerToken(token);
        
        return Post(request, cancellationToken);
    }
    
    public Task<FriendsRequestShort> AddRequestAsync(
        string token,
        string userId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.RequestRoute)
            .AddQueryParameter(nameof(userId), userId)
            .AddBearerToken(token);
        
        return Post<FriendsRequestShort>(request, cancellationToken);
    }
    
    public Task DeleteRequestAsync(
        string token,
        Guid requestId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{KnownRoutes.RequestRoute}/{requestId}")
            .AddBearerToken(token);
        
        return Delete(request, cancellationToken);
    }
}