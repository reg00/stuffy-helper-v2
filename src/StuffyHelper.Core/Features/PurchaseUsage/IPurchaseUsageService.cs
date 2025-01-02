using StuffyHelper.Common.Contracts;
using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.PurchaseUsage
{
    public interface IPurchaseUsageService
    {
        Task<GetPurchaseUsageEntry> GetPurchaseUsageAsync(StuffyClaims claims, Guid purchaseUsageId, CancellationToken cancellationToken);

        Task<Response<PurchaseUsageShortEntry>> GetPurchaseUsagesAsync(
            int offset = 0,
            int limit = 10,
            Guid? eventId = null,
            Guid? participantId = null,
            Guid? purchaseId = null,
            CancellationToken cancellationToken = default);

        Task<PurchaseUsageShortEntry> AddPurchaseUsageAsync(UpsertPurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default);

        Task DeletePurchaseUsageAsync(Guid purchaseUsageId, CancellationToken cancellationToken = default);

        Task<PurchaseUsageShortEntry> UpdatePurchaseUsageAsync(Guid purchaseUsageId, UpsertPurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default);
    }
}
