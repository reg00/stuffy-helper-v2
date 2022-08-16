using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.PurchaseType
{
    public interface IPurchaseTypeStore
    {
        Task<PurchaseTypeEntry> GetPurchaseTypeAsync(Guid purchaseTypeId, CancellationToken cancellationToken);

        Task<Response<PurchaseTypeEntry>> GetPurchaseTypesAsync(
            int offset = 0,
            int limit = 10,
            string name = null,
            Guid? purchaseId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default);

        Task<PurchaseTypeEntry> AddPurchaseTypeAsync(PurchaseTypeEntry purchaseType, CancellationToken cancellationToken = default);

        Task DeletePurchaseTypeAsync(Guid purchaseTypeId, CancellationToken cancellationToken = default);

        Task<PurchaseTypeEntry> UpdatePurchaseTypeAsync(PurchaseTypeEntry purchaseType, CancellationToken cancellationToken = default);
    }
}
