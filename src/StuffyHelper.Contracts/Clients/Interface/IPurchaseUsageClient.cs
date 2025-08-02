using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients.Interface;

public interface IPurchaseUsageClient
{
    public Task<Response<PurchaseUsageShortEntry>> GetPurchaseUsagesAsync(
        string token,
        int offset = 0,
        int limit = 10,
        Guid? eventId = null,
        Guid? participantId = null,
        Guid? purchaseId = null,
        CancellationToken cancellationToken = default);

    public Task<GetPurchaseUsageEntry> GetPurchaseUsageAsync(
        string token,
        Guid purchaseUsageId,
        CancellationToken cancellationToken = default);

    public Task<PurchaseUsageShortEntry> CreatePurchaseUsageAsync(
        string token,
        UpsertPurchaseUsageEntry body,
        CancellationToken cancellationToken = default);

    public Task DeletePurchaseUsageAsync(
        string token,
        Guid purchaseUsageId,
        CancellationToken cancellationToken = default);

    public Task<PurchaseUsageShortEntry> UpdatePurchaseUsageAsync(
        string token,
        Guid purchaseUsageId,
        UpsertPurchaseUsageEntry body,
        CancellationToken cancellationToken = default);
}