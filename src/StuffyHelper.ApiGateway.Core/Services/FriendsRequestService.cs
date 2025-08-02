using System.Security.Claims;
using StuffyHelper.ApiGateway.Core.Services.Interfaces;
using StuffyHelper.Authorization.Contracts.Clients.Interface;
using StuffyHelper.Authorization.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services;

/// <inheritdoc />
public class FriendsRequestService : IFriendsRequestService
{
    private readonly IFriendRequestClient _friendRequestClient;

    /// <summary>
    /// Ctor.
    /// </summary>
    public FriendsRequestService(IFriendRequestClient friendRequestClient)
    {
        _friendRequestClient = friendRequestClient;
    }

    /// <inheritdoc />
    public async Task ConfirmRequest(string token, Guid requestId, CancellationToken cancellationToken = default)
    {
        await _friendRequestClient.ConfirmAsync(token, requestId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<FriendsRequestShort> GetRequestAsync(string token, Guid requestId,
        CancellationToken cancellationToken)
    {
        return await _friendRequestClient.GetAsync(token, requestId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<FriendsRequestShort>> GetSentRequestsAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        return await _friendRequestClient.GetSentRequestsAsync(token, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<FriendsRequestShort>> GetIncomingRequestsAsync(
        string token,
        CancellationToken cancellationToken = default)
    {
        return await _friendRequestClient.GetIncomingRequestsAsync(token, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<FriendsRequestShort> AddRequestAsync(
        string token,
        string userId,
        CancellationToken cancellationToken = default)
    {
        return await _friendRequestClient.AddRequestAsync(token, userId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteRequestAsync(string token, Guid requestId, CancellationToken cancellationToken = default)
    {
        await _friendRequestClient.DeleteRequestAsync(token, requestId, cancellationToken);
    }
}