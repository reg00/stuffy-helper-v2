using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients.Interface;

public interface IPurchaseUsageClient
{
    /// <summary>
    /// Получение списка того, какие участники какие продукты используют
    /// </summary>
    public Task<Response<PurchaseUsageShortEntry>> GetPurchaseUsagesAsync(
        string token,
        int offset = 0,
        int limit = 10,
        Guid? eventId = null,
        Guid? participantId = null,
        Guid? purchaseId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение данных об использовании участником продукта по идентификатору
    /// </summary>
    public Task<GetPurchaseUsageEntry> GetPurchaseUsageAsync(
        string token,
        Guid purchaseUsageId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавление использования участником продукта
    /// </summary>
    public Task<PurchaseUsageShortEntry> CreatePurchaseUsageAsync(
        string token,
        UpsertPurchaseUsageEntry body,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление использования участником продукта
    /// </summary>
    public Task DeletePurchaseUsageAsync(
        string token,
        Guid purchaseUsageId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Узменение использования участником продукта (а надо ли??)
    /// </summary>
    public Task<PurchaseUsageShortEntry> UpdatePurchaseUsageAsync(
        string token,
        Guid purchaseUsageId,
        UpsertPurchaseUsageEntry body,
        CancellationToken cancellationToken = default);
}