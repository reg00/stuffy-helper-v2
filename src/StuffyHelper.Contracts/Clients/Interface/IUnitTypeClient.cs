using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients.Interface;

public interface IUnitTypeClient
{
    /// <summary>
    /// Получение списка единиц измерения
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
    /// Получение информации о единице измерения по идентификатору
    /// </summary>
    public Task<GetUnitTypeEntry> GetUnitTypeAsync(
        string token,
        Guid unitTypeId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавление единицы измерения
    /// </summary>
    public Task<UnitTypeShortEntry> CreateUnitTypeAsync(
        string token,
        UpsertUnitTypeEntry body,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление единицы измерения
    /// </summary>
    public Task DeleteUnitTypeAsync(
        string token,
        Guid unitTypeId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Изменение единицы измерения
    /// </summary>
    public Task<UnitTypeShortEntry> UpdateUnitTypeAsync(
        string token,
        Guid unitTypeId,
        UpsertUnitTypeEntry body,
        CancellationToken cancellationToken = default);
}