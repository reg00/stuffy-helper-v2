using StuffyHelper.Contracts.Models;
using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.PurchaseTag
{
    public interface IPurchaseTagService
    {
        Task<GetPurchaseTagEntry> GetPurchaseTagAsync(Guid purchaseTagId, CancellationToken cancellationToken);

        Task<Response<PurchaseTagShortEntry>> GetPurchaseTagsAsync(
            int offset = 0,
            int limit = 10,
            string? name = null,
            Guid? purchaseId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default);

        Task<PurchaseTagShortEntry> AddPurchaseTagAsync(UpsertPurchaseTagEntry purchaseTag, CancellationToken cancellationToken = default);

        Task DeletePurchaseTagAsync(Guid purchaseTagId, CancellationToken cancellationToken = default);

        Task<PurchaseTagShortEntry> UpdatePurchaseTagAsync(Guid purchaseTagId, UpsertPurchaseTagEntry purchaseTag, CancellationToken cancellationToken = default);
    }
}
