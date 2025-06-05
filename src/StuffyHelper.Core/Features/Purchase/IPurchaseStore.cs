using StuffyHelper.Contracts.Entities;
using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.Purchase
{
    public interface IPurchaseStore
    {
        Task<PurchaseEntry> GetPurchaseAsync(Guid purchaseId, CancellationToken cancellationToken);

        Task<Response<PurchaseEntry>> GetPurchasesAsync(
            int offset = 0,
            int limit = 10,
            string? name = null,
            double? costMin = null,
            double? costMax = null,
            Guid? eventId = null,
            IEnumerable<string>? purchaseTags = null,
            Guid? unitTypeId = null,
            bool? isComplete = null,
            CancellationToken cancellationToken = default);

        Task<PurchaseEntry> AddPurchaseAsync(PurchaseEntry purchase, CancellationToken cancellationToken = default);

        Task DeletePurchaseAsync(Guid purchaseId, CancellationToken cancellationToken = default);

        Task<PurchaseEntry> UpdatePurchaseAsync(PurchaseEntry purchase, CancellationToken cancellationToken = default);

        Task CompletePurchaseAsync(Guid purchaseId, CancellationToken cancellationToken = default);
    }
}
