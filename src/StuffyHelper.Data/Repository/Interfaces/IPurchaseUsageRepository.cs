using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Data.Repository.Interfaces
{
    public interface IPurchaseUsageRepository
    {
        Task<PurchaseUsageEntry> GetPurchaseUsageAsync(Guid purchaseUsageId, CancellationToken cancellationToken);

        Task<Response<PurchaseUsageEntry>> GetPurchaseUsagesAsync(
            int offset = 0,
            int limit = 10,
            Guid? eventId = null,
            Guid? participantId = null,
            Guid? purchaseId = null,
            CancellationToken cancellationToken = default);

        Task<PurchaseUsageEntry> AddPurchaseUsageAsync(PurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default);

        Task DeletePurchaseUsageAsync(Guid purchaseUsageId, CancellationToken cancellationToken = default);

        Task<PurchaseUsageEntry> UpdatePurchaseUsageAsync(PurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default);
    }
}
