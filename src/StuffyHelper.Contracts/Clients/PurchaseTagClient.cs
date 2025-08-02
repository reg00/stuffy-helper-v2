using RestSharp;
using StuffyHelper.Common.Client;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;
using StuffyHelper.Contracts.Clients.Interface;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients;

public class PurchaseTagClient: ApiClientBase, IPurchaseTagClient
{
    public PurchaseTagClient(string baseUrl) : base(baseUrl)
    {
        
    }
    
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
    
    public Task<GetPurchaseTagEntry> GetPurchaseTagAsync(
        string token,
        Guid purchaseTagId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.GetPurchaseTagRoute)
            .AddBearerToken(token);

        return Get<GetPurchaseTagEntry>(request, cancellationToken);
    }
    
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
    
    public Task DeletePurchaseTagAsync(
        string token,
        Guid purchaseTagId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.DeletePurchaseTagRoute)
            .AddBearerToken(token);

        return Delete(request, cancellationToken);
    }
    
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