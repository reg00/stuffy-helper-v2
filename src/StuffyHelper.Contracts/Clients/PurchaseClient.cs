using RestSharp;
using StuffyHelper.Common.Client;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Clients.Interface;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients;

/// <inheritdoc cref="StuffyHelper.Contracts.Clients.Interface.IPurchaseClient" />
public class PurchaseClient: ApiClientBase, IPurchaseClient
{
    private const string DefaultRoute = "api/v1";
    
    /// <summary>
    /// Ctor.
    /// </summary>
    public PurchaseClient(string baseUrl) : base(baseUrl)
    {
        
    }
    
    /// <inheritdoc />
    public Task<Response<GetPurchaseEntry>> GetPurchasesAsync(
        string token,
        Guid eventId,
        int offset = 0,
        int limit = 10,
        string? name = null,
        double? costMin = null,
        double? costMax = null,
        string[]? purchaseTags = null,
        Guid? unitTypeId = null,
        bool? isComplete = null,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/events/{eventId}/purchases")
            .AddBearerToken(token)
            .AddQueryParameter("limit", limit)
            .AddQueryParameter("offset", offset)
            .AddOptionalQueryParameter(nameof(name), name)
            .AddOptionalQueryParameter(nameof(costMin), costMin)
            .AddOptionalQueryParameter(nameof(costMax), costMax)
            .AddOptionalQueryParameter(nameof(purchaseTags), purchaseTags)
            .AddOptionalQueryParameter(nameof(unitTypeId), unitTypeId)
            .AddOptionalQueryParameter(nameof(isComplete), isComplete);

        return Get<Response<GetPurchaseEntry>>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<GetPurchaseEntry> GetPurchaseAsync(
        string token,
        Guid eventId,
        Guid purchaseId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/events/{eventId}/purchases/{purchaseId}")
            .AddBearerToken(token);

        return Get<GetPurchaseEntry>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<PurchaseShortEntry> CreatePurchaseAsync(
        string token,
        Guid eventId,
        AddPurchaseEntry body,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/events/{eventId}/purchases")
            .AddBearerToken(token)
            .AddJsonBody(body);

        return Post<PurchaseShortEntry>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task DeletePurchaseAsync(
        string token,
        Guid eventId,
        Guid purchaseId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/events/{eventId}/purchases/{purchaseId}")
            .AddBearerToken(token);

        return Delete(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<PurchaseShortEntry> UpdatePurchaseAsync(
        string token,
        Guid eventId,
        Guid purchaseId,
        UpdatePurchaseEntry body,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/events/{eventId}/purchases/{purchaseId}")
            .AddBearerToken(token)
            .AddJsonBody(body);

        return Patch<PurchaseShortEntry>(request, cancellationToken);
    }
}