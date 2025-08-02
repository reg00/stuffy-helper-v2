using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients.Interface;

public interface IPurchaseTagClient
{
    public Task<Response<PurchaseTagShortEntry>> GetPurchaseTagsAsync(
        string token,
        int offset = 0,
        int limit = 10,
        string? name = null,
        Guid? purchaseId = null,
        bool? isActive = null,
        CancellationToken cancellationToken = default);

    public Task<GetPurchaseTagEntry> GetPurchaseTagAsync(
        string token,
        Guid purchaseTagId,
        CancellationToken cancellationToken = default);

    public Task<PurchaseTagShortEntry> CreatePurchaseTagAsync(
        string token,
        UpsertPurchaseTagEntry body,
        CancellationToken cancellationToken = default);

    public Task DeletePurchaseTagAsync(
        string token,
        Guid purchaseTagId,
        CancellationToken cancellationToken = default);

    public Task<PurchaseTagShortEntry> UpdatePurchaseTagAsync(
        string token,
        Guid purchaseTagId,
        UpsertPurchaseTagEntry body,
        CancellationToken cancellationToken = default);
}