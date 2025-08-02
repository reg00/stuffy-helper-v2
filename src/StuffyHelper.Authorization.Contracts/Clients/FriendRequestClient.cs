using RestSharp;
using StuffyHelper.Authorization.Contracts.Clients.Interface;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Client;
using StuffyHelper.Common.Web;

namespace StuffyHelper.Authorization.Contracts.Clients;

/// <inheritdoc cref="StuffyHelper.Authorization.Contracts.Clients.Interface.IFriendRequestClient" />
public class FriendRequestClient : ApiClientBase, IFriendRequestClient
{
    /// <inheritdoc />
    public FriendRequestClient(string baseUrl) : base(baseUrl)
    {
        
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<FriendsRequestShort>> GetSentRequestsAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.GetSendedRequestsRoute)
            .AddBearerToken(token);

        return Get<IReadOnlyList<FriendsRequestShort>>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<IReadOnlyList<FriendsRequestShort>> GetIncomingRequestsAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.GetIncomingRequestsRoute)
            .AddBearerToken(token);

        return Get<IReadOnlyList<FriendsRequestShort>>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<FriendsRequestShort> GetAsync(
        string token,
        Guid requestId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{KnownRoutes.RequestRoute}/{requestId}")
            .AddBearerToken(token);
        
        return Get<FriendsRequestShort>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task ConfirmAsync(
        string token,
        Guid requestId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{KnownRoutes.RequestRoute}/{requestId}/accept")
            .AddBearerToken(token);
        
        return Post(request, cancellationToken);
    }
    
    /// <inheritdoc />
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
    
    /// <inheritdoc />
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