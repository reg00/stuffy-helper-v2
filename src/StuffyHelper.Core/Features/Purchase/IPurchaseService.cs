using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.Purchase
{
    public interface IPurchaseService
    {
        Task<GetPurchaseEntry> GetPurchaseAsync(Guid purchaseId, CancellationToken cancellationToken);

        Task<Response<PurchaseShortEntry>> GetPurchasesAsync(
            int offset = 0,
            int limit = 10,
            string? name = null,
            double? costMin = null,
            double? costMax = null,
            Guid? eventId = null,
            IEnumerable<string>? purchaseTags = null,
            Guid? unitTypeId = null,
            CancellationToken cancellationToken = default);

        Task<PurchaseShortEntry> AddPurchaseAsync(AddPurchaseEntry purchase, CancellationToken cancellationToken = default);

        Task DeletePurchaseAsync(Guid purchaseId, CancellationToken cancellationToken = default);

        Task<PurchaseShortEntry> UpdatePurchaseAsync(Guid purchaseId, UpdatePurchaseEntry purchase, CancellationToken cancellationToken = default);
    }
}
