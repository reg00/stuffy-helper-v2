using RestSharp;
using StuffyHelper.Common.Client;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;
using StuffyHelper.Contracts.Clients.Interface;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients;

/// <inheritdoc cref="StuffyHelper.Contracts.Clients.Interface.IPurchaseTagClient" />
public class PurchaseTagClient: ApiClientBase, IPurchaseTagClient
{
    /// <summary>
    /// Ctor.
    /// </summary>
    public PurchaseTagClient(string baseUrl) : base(baseUrl)
    {
        
    }
    
    /// <inheritdoc />
     public Task<Response<PurchaseTagShortEntry>> GetPurchaseTagsAsync(
        string token,
        int offset = 0,
        int limit = 10,
        string? name = null,
        Guid? purchaseId = null,
        bool? isActive = null,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.GetPurchaseTagsRoute)
            .AddBearerToken(token)
            .AddQueryParameter("limit", limit)
            .AddQueryParameter("offset", offset)
            .AddOptionalQueryParameter(nameof(name), name)
            .AddOptionalQueryParameter(nameof(purchaseId), purchaseId)
            .AddOptionalQueryParameter(nameof(isActive), isActive);

        return Get<Response<PurchaseTagShortEntry>>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<GetPurchaseTagEntry> GetPurchaseTagAsync(
        string token,
        Guid purchaseTagId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.GetPurchaseTagRoute)
            .AddBearerToken(token);

        return Get<GetPurchaseTagEntry>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<PurchaseTagShortEntry> CreatePurchaseTagAsync(
        string token,
        UpsertPurchaseTagEntry body,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.AddPurchaseTagRoute)
            .AddBearerToken(token)
            .AddJsonBody(body);

        return Post<PurchaseTagShortEntry>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task DeletePurchaseTagAsync(
        string token,
        Guid purchaseTagId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.DeletePurchaseTagRoute)
            .AddBearerToken(token);

        return Delete(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<PurchaseTagShortEntry> UpdatePurchaseTagAsync(
        string token,
        Guid purchaseTagId,
        UpsertPurchaseTagEntry body,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.UpdatePurchaseTagRoute)
            .AddBearerToken(token)
            .AddJsonBody(body);

        return Patch<PurchaseTagShortEntry>(request, cancellationToken);
    }
}