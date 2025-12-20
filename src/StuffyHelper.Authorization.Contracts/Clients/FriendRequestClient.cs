using RestSharp;
using StuffyHelper.Authorization.Contracts.Clients.Interface;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Client;

namespace StuffyHelper.Authorization.Contracts.Clients;

/// <inheritdoc cref="StuffyHelper.Authorization.Contracts.Clients.Interface.IFriendRequestClient" />
public class FriendRequestClient : ApiClientBase, IFriendRequestClient
{
    private const string DefaultRoute = "api/v1/requests";
    
    /// <inheritdoc />
    public FriendRequestClient(string baseUrl) : base(baseUrl)
    {
        
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<FriendsRequestShort>> GetSentRequestsAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/sended")
            .AddBearerToken(token);

        return Get<IReadOnlyList<FriendsRequestShort>>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<IReadOnlyList<FriendsRequestShort>> GetIncomingRequestsAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/incoming")
            .AddBearerToken(token);

        return Get<IReadOnlyList<FriendsRequestShort>>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<FriendsRequestShort> GetAsync(
        string token,
        Guid requestId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/{requestId}")
            .AddBearerToken(token);
        
        return Get<FriendsRequestShort>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task ConfirmAsync(
        string token,
        Guid requestId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/{requestId}/accept")
            .AddBearerToken(token);
        
        return Post(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<FriendsRequestShort> AddRequestAsync(
        string token,
        string userId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(DefaultRoute)
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
        var request = CreateRequest($"{DefaultRoute}/{requestId}")
            .AddBearerToken(token);
        
        return Delete(request, cancellationToken);
    }
}