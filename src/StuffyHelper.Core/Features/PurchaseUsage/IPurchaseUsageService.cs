using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.PurchaseUsage
{
    public interface IPurchaseUsageService
    {
        Task<GetPurchaseUsageEntry> GetPurchaseUsageAsync(Guid purchaseUsageId, CancellationToken cancellationToken);

        Task<Response<GetPurchaseUsageEntry>> GetPurchaseUsagesAsync(
            int offset = 0,
            int limit = 10,
            Guid? participantId = null,
            Guid? purchaseId = null,
            CancellationToken cancellationToken = default);

        Task<GetPurchaseUsageEntry> AddPurchaseUsageAsync(UpsertPurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default);

        Task DeletePurchaseUsageAsync(Guid purchaseUsageId, CancellationToken cancellationToken = default);

        Task<GetPurchaseUsageEntry> UpdatePurchaseUsageAsync(Guid purchaseUsageId, UpsertPurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default);
    }
}
