using RestSharp;
using StuffyHelper.Common.Client;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Clients.Interface;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients;

/// <inheritdoc cref="StuffyHelper.Contracts.Clients.Interface.IDebtClient" />
public class DebtClient: ApiClientBase, IDebtClient
{
    private const string DefaultRoute = "api/v1";
    
    /// <summary>
    /// Ctor.
    /// </summary>
    public DebtClient(string baseUrl) : base(baseUrl)
    {
        
    }
    
    /// <inheritdoc />
    public Task<Response<GetDebtEntry>> GetDebtsAsync(
        string token,
        Guid eventId,
        int offset = 0,
        int limit = 10,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/events/{eventId}/debts")
            .AddBearerToken(token)
            .AddQueryParameter("limit", limit)
            .AddQueryParameter("offset", offset);

        return Get<Response<GetDebtEntry>>(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task<GetDebtEntry> GetDebtAsync(
        string token,
        Guid eventId,
        Guid debtId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/events/{eventId}/debts/{debtId}")
            .AddBearerToken(token);

        return Get<GetDebtEntry>(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task SendDebtAsync(string token, Guid eventId, Guid debtId, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/events/{eventId}/debts/{debtId}/send")
            .AddBearerToken(token);

        return Post(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task ConfirmDebtAsync(string token, Guid eventId, Guid debtId, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/events/{eventId}/debts/{debtId}/confirm")
            .AddBearerToken(token);

        return Post(request, cancellationToken);
    }
}