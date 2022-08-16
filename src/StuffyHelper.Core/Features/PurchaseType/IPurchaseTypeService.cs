using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.PurchaseType
{
    public interface IPurchaseTypeService
    {
        Task<GetPurchaseTypeEntry> GetPurchaseTypeAsync(Guid purchaseTypeId, CancellationToken cancellationToken);

        Task<Response<GetPurchaseTypeEntry>> GetPurchaseTypesAsync(
            int offset = 0,
            int limit = 10,
            string name = null,
            Guid? purchaseId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default);

        Task<GetPurchaseTypeEntry> AddPurchaseTypeAsync(UpsertPurchaseTypeEntry purchaseType, CancellationToken cancellationToken = default);

        Task DeletePurchaseTypeAsync(Guid purchaseTypeId, CancellationToken cancellationToken = default);

        Task<GetPurchaseTypeEntry> UpdatePurchaseTypeAsync(Guid purchaseTypeId, UpsertPurchaseTypeEntry purchaseType, CancellationToken cancellationToken = default);
    }
}
