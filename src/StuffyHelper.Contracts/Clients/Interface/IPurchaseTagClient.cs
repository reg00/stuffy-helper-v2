using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients.Interface;

public interface IPurchaseTagClient
{
    /// <summary>
    /// Получение списка тэгов покупок
    /// </summary>
    public Task<Response<PurchaseTagShortEntry>> GetPurchaseTagsAsync(
        string token,
        int offset = 0,
        int limit = 10,
        string? name = null,
        Guid? purchaseId = null,
        bool? isActive = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение данных тэга покупки
    /// </summary>
    public Task<GetPurchaseTagEntry> GetPurchaseTagAsync(
        string token,
        Guid purchaseTagId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавление тэга покупки
    /// </summary>
    public Task<PurchaseTagShortEntry> CreatePurchaseTagAsync(
        string token,
        UpsertPurchaseTagEntry body,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление тэга покупки
    /// </summary>
    public Task DeletePurchaseTagAsync(
        string token,
        Guid purchaseTagId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Изменение тэга покупки
    /// </summary>
    public Task<PurchaseTagShortEntry> UpdatePurchaseTagAsync(
        string token,
        Guid purchaseTagId,
        UpsertPurchaseTagEntry body,
        CancellationToken cancellationToken = default);
}