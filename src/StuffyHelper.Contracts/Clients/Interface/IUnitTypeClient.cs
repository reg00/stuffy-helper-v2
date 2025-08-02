using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients.Interface;

public interface IUnitTypeClient
{
    public Task<Response<UnitTypeShortEntry>> GetUnitTypesAsync(
        string token,
        int offset = 0,
        int limit = 10,
        string? name = null,
        Guid? purchaseId = null,
        bool? isActive = null,
        CancellationToken cancellationToken = default);

    public Task<GetUnitTypeEntry> GetUnitTypeAsync(
        string token,
        Guid unitTypeId,
        CancellationToken cancellationToken = default);

    public Task<UnitTypeShortEntry> CreateUnitTypeAsync(
        string token,
        UpsertUnitTypeEntry body,
        CancellationToken cancellationToken = default);

    public Task DeleteUnitTypeAsync(
        string token,
        Guid unitTypeId,
        CancellationToken cancellationToken = default);

    public Task<UnitTypeShortEntry> UpdateUnitTypeAsync(
        string token,
        Guid unitTypeId,
        UpsertUnitTypeEntry body,
        CancellationToken cancellationToken = default);
}