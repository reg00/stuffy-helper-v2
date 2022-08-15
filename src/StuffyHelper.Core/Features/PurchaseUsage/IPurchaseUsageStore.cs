using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.PurchaseUsage
{
    public interface IPurchaseUsageStore
    {
        Task<PurchaseUsageEntry> GetPurchaseUsageAsync(Guid purchaseUsageId, CancellationToken cancellationToken);

        Task<Response<PurchaseUsageEntry>> GetPurchaseUsagesAsync(
            int offset = 0,
            int limit = 10,
            Guid? participantId = null,
            Guid? purchaseId = null,
            CancellationToken cancellationToken = default);

        Task<PurchaseUsageEntry> AddPurchaseUsageAsync(PurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default);

        Task DeletePurchaseUsageAsync(Guid purchaseUsageId, CancellationToken cancellationToken = default);

        Task<PurchaseUsageEntry> UpdatePurchaseUsageAsync(PurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default);
    }
}
