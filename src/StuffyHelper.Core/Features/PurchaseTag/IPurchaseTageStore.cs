using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.PurchaseTag
{
    public interface IPurchaseTagStore
    {
        Task<PurchaseTagEntry> GetPurchaseTagAsync(Guid purchaseTagId, CancellationToken cancellationToken);
        Task<PurchaseTagEntry> GetPurchaseTagAsync(string tag, CancellationToken cancellationToken);

        Task<Response<PurchaseTagEntry>> GetPurchaseTagsAsync(
            int offset = 0,
            int limit = 10,
            string? name = null,
            Guid? purchaseId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default);

        Task<PurchaseTagEntry> AddPurchaseTagAsync(PurchaseTagEntry purchaseTag, CancellationToken cancellationToken = default);

        Task DeletePurchaseTagAsync(Guid purchaseTagId, CancellationToken cancellationToken = default);

        Task<PurchaseTagEntry> UpdatePurchaseTagAsync(PurchaseTagEntry purchaseTag, CancellationToken cancellationToken = default);
    }
}
