using RestSharp;
using StuffyHelper.Authorization.Contracts.Clients.Interface;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Client;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;

namespace StuffyHelper.Authorization.Contracts.Clients;

public class FriendClient: ApiClientBase, IFriendClient
{
    public FriendClient(string baseUrl) : base(baseUrl)
    {
        
    }
    
    public Task<Response<UserShortEntry>> GetAsync(
        string token,
        int limit = 20,
        int offset = 0,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.GetFriendsRoute)
            .AddQueryParameter(nameof(limit), limit)
            .AddQueryParameter(nameof(offset), offset)
            .AddBearerToken(token);
        
        return Get<Response<UserShortEntry>>(request, cancellationToken);
    }
}