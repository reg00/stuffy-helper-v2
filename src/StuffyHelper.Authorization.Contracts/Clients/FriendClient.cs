using RestSharp;
using StuffyHelper.Authorization.Contracts.Clients.Interface;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Client;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;

namespace StuffyHelper.Authorization.Contracts.Clients;

/// <inheritdoc cref="StuffyHelper.Authorization.Contracts.Clients.Interface.IFriendClient" />
public class FriendClient: ApiClientBase, IFriendClient
{
    private const string DefaultRoute = "api/v1/friends";
    
    /// <inheritdoc />
    public FriendClient(string baseUrl) : base(baseUrl)
    {
        
    }
    
    /// <inheritdoc />
    public Task<Response<UserShortEntry>> GetAsync(
        string token,
        int limit = 20,
        int offset = 0,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(DefaultRoute)
            .AddQueryParameter(nameof(limit), limit)
            .AddQueryParameter(nameof(offset), offset)
            .AddBearerToken(token);
        
        return Get<Response<UserShortEntry>>(request, cancellationToken);
    }
}