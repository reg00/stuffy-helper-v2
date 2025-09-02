using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients.Interface;

/// <summary>
/// Interface for work with unit types
/// </summary>
public interface IUnitTypeClient
{
    /// <summary>
    /// Return list of unit types
    /// </summary>
    public Task<Response<UnitTypeShortEntry>> GetUnitTypesAsync(
        string token,
        int offset = 0,
        int limit = 10,
        string? name = null,
        Guid? purchaseId = null,
        bool? isActive = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Return unit type by id
    /// </summary>
    public Task<GetUnitTypeEntry> GetUnitTypeAsync(
        string token,
        Guid unitTypeId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Create new unit type
    /// </summary>
    public Task<UnitTypeShortEntry> CreateUnitTypeAsync(
        string token,
        UpsertUnitTypeEntry body,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove unit type
    /// </summary>
    public Task DeleteUnitTypeAsync(
        string token,
        Guid unitTypeId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Update unit type data
    /// </summary>
    public Task<UnitTypeShortEntry> UpdateUnitTypeAsync(
        string token,
        Guid unitTypeId,
        UpsertUnitTypeEntry body,
        CancellationToken cancellationToken = default);
}