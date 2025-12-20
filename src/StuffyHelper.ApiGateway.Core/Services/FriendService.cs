using StuffyHelper.ApiGateway.Core.Services.Interfaces;
using StuffyHelper.Authorization.Contracts.Clients.Interface;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Messages;

namespace StuffyHelper.ApiGateway.Core.Services;

/// <inheritdoc />
public class FriendService : IFriendService
{
    private readonly IFriendClient _friendClient;
        
        /// <summary>
        /// Ctor.
        /// </summary>
        public FriendService(IFriendClient friendClient)
        {
            _friendClient = friendClient;
        }


        /// <inheritdoc />
        public async Task<Response<UserShortEntry>> GetFriends(string token, int limit = 20, int offset = 0, CancellationToken cancellationToken = default)
        {
            return await _friendClient.GetAsync(token, limit, offset, cancellationToken);
        }
    }