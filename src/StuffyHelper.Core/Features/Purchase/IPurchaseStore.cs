using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.Purchase
{
    public interface IPurchaseStore
    {
        Task<PurchaseEntry> GetPurchaseAsync(Guid purchaseId, CancellationToken cancellationToken);

        Task<Response<PurchaseEntry>> GetPurchasesAsync(
            int offset = 0,
            int limit = 10,
            string name = null,
            int? countMin = null,
            int? countMax = null,
            double? amountMin = null,
            double? amountMax = null,
            double? weightMin = null,
            double? weightMax = null,
            Guid? shoppingId = null,
            Guid? purchaseTypeId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default);

        Task<PurchaseEntry> AddPurchaseAsync(PurchaseEntry purchase, CancellationToken cancellationToken = default);

        Task DeletePurchaseAsync(Guid purchaseId, CancellationToken cancellationToken = default);

        Task<PurchaseEntry> UpdatePurchaseAsync(PurchaseEntry purchase, CancellationToken cancellationToken = default);
    }
}
