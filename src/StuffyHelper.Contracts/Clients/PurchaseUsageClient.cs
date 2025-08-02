using RestSharp;
using StuffyHelper.Common.Client;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;
using StuffyHelper.Contracts.Clients.Interface;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients;

/// <inheritdoc cref="StuffyHelper.Contracts.Clients.Interface.IPurchaseUsageClient" />
public class PurchaseUsageClient: ApiClientBase, IPurchaseUsageClient
{
    /// <summary>
    /// Ctor.
    /// </summary>
    public PurchaseUsageClient(string baseUrl) : base(baseUrl)
    {
        
    }
    
    /// <inheritdoc />
    public Task<Response<PurchaseUsageShortEntry>> GetPurchaseUsagesAsync(
        string token,
        int offset = 0,
        int limit = 10,
        Guid? eventId = null,
        Guid? participantId = null,
        Guid? purchaseId = null,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.GetPurchaseUsagesRoute)
            .AddBearerToken(token)
            .AddQueryParameter("limit", limit)
            .AddQueryParameter("offset", offset)
            .AddOptionalQueryParameter(nameof(eventId), eventId)
            .AddOptionalQueryParameter(nameof(purchaseId), purchaseId)
            .AddOptionalQueryParameter(nameof(participantId), participantId);

        return Get<Response<PurchaseUsageShortEntry>>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<GetPurchaseUsageEntry> GetPurchaseUsageAsync(
        string token,
        Guid purchaseUsageId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.GetPurchaseUsageRoute)
            .AddBearerToken(token);

        return Get<GetPurchaseUsageEntry>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<PurchaseUsageShortEntry> CreatePurchaseUsageAsync(
        string token,
        UpsertPurchaseUsageEntry body,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.AddPurchaseUsageRoute)
            .AddBearerToken(token)
            .AddJsonBody(body);

        return Post<PurchaseUsageShortEntry>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task DeletePurchaseUsageAsync(
        string token,
        Guid purchaseUsageId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.DeletePurchaseUsageRoute)
            .AddBearerToken(token);

        return Delete(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<PurchaseUsageShortEntry> UpdatePurchaseUsageAsync(
        string token,
        Guid purchaseUsageId,
        UpsertPurchaseUsageEntry body,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.UpdatePurchaseUsageRoute)
            .AddBearerToken(token)
            .AddJsonBody(body);

        return Patch<PurchaseUsageShortEntry>(request, cancellationToken);
    }
}