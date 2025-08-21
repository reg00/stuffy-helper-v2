using RestSharp;
using StuffyHelper.Common.Client;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Clients.Interface;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients;

/// <inheritdoc cref="StuffyHelper.Contracts.Clients.Interface.IUnitTypeClient" />
public class UnitTypeClient: ApiClientBase, IUnitTypeClient
{
    private const string DefaultRoute = "api/v1/unit-types";
    
    /// <summary>
    /// Ctor.
    /// </summary>
    public UnitTypeClient(string baseUrl) : base(baseUrl)
    {
        
    }
    
    /// <inheritdoc />
    public Task<Response<UnitTypeShortEntry>> GetUnitTypesAsync(
        string token,
        int offset = 0,
        int limit = 10,
        string? name = null,
        Guid? purchaseId = null,
        bool? isActive = null,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(DefaultRoute)
            .AddBearerToken(token)
            .AddQueryParameter("limit", limit)
            .AddQueryParameter("offset", offset)
            .AddOptionalQueryParameter(nameof(name), name)
            .AddOptionalQueryParameter(nameof(purchaseId), purchaseId)
            .AddOptionalQueryParameter(nameof(isActive), isActive);

        return Get<Response<UnitTypeShortEntry>>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<GetUnitTypeEntry> GetUnitTypeAsync(
        string token,
        Guid unitTypeId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/{unitTypeId}")
            .AddBearerToken(token);

        return Get<GetUnitTypeEntry>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<UnitTypeShortEntry> CreateUnitTypeAsync(
        string token,
        UpsertUnitTypeEntry body,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(DefaultRoute)
            .AddBearerToken(token)
            .AddJsonBody(body);

        return Post<UnitTypeShortEntry>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task DeleteUnitTypeAsync(
        string token,
        Guid unitTypeId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/{unitTypeId}")
            .AddBearerToken(token);

        return Delete(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<UnitTypeShortEntry> UpdateUnitTypeAsync(
        string token,
        Guid unitTypeId,
        UpsertUnitTypeEntry body,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/{unitTypeId}")
            .AddBearerToken(token)
            .AddJsonBody(body);

        return Patch<UnitTypeShortEntry>(request, cancellationToken);
    }
}