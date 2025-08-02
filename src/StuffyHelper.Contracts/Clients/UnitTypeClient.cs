using RestSharp;
using StuffyHelper.Common.Client;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;
using StuffyHelper.Contracts.Clients.Interface;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients;

public class UnitTypeClient: ApiClientBase, IUnitTypeClient
{
    public UnitTypeClient(string baseUrl) : base(baseUrl)
    {
        
    }
    
    public Task<Response<UnitTypeShortEntry>> GetUnitTypesAsync(
        string token,
        int offset = 0,
        int limit = 10,
        string? name = null,
        Guid? purchaseId = null,
        bool? isActive = null,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.GetUnitTypesRoute)
            .AddBearerToken(token)
            .AddQueryParameter("limit", limit)
            .AddQueryParameter("offset", offset)
            .AddOptionalQueryParameter(nameof(name), name)
            .AddOptionalQueryParameter(nameof(purchaseId), purchaseId)
            .AddOptionalQueryParameter(nameof(isActive), isActive);

        return Get<Response<UnitTypeShortEntry>>(request, cancellationToken);
    }
    
    public Task<GetUnitTypeEntry> GetUnitTypeAsync(
        string token,
        Guid unitTypeId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.GetUnitTypeRoute)
            .AddBearerToken(token);

        return Get<GetUnitTypeEntry>(request, cancellationToken);
    }
    
    public Task<UnitTypeShortEntry> CreateUnitTypeAsync(
        string token,
        UpsertUnitTypeEntry body,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.AddUnitTypeRoute)
            .AddBearerToken(token)
            .AddJsonBody(body);

        return Post<UnitTypeShortEntry>(request, cancellationToken);
    }
    
    public Task DeleteUnitTypeAsync(
        string token,
        Guid unitTypeId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.DeleteUnitTypeRoute)
            .AddBearerToken(token);

        return Delete(request, cancellationToken);
    }
    
    public Task<UnitTypeShortEntry> UpdateUnitTypeAsync(
        string token,
        Guid unitTypeId,
        UpsertUnitTypeEntry body,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.UpdateUnitTypeRoute)
            .AddBearerToken(token)
            .AddJsonBody(body);

        return Patch<UnitTypeShortEntry>(request, cancellationToken);
    }
}