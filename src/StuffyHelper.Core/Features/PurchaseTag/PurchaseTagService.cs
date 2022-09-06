using EnsureThat;
using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.PurchaseTag
{
    public class PurchaseTagService : IPurchaseTagService
    {
        private readonly IPurchaseTagStore _PurchaseTagStore;

        public PurchaseTagService(IPurchaseTagStore PurchaseTagStore)
        {
            _PurchaseTagStore = PurchaseTagStore;
        }

        public async Task<GetPurchaseTagEntry> GetPurchaseTagAsync(Guid purchaseTagId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(purchaseTagId, nameof(purchaseTagId));

            var entry = await _PurchaseTagStore.GetPurchaseTagAsync(purchaseTagId, cancellationToken);

            return new GetPurchaseTagEntry(entry, true);
        }

        public async Task<Response<GetPurchaseTagEntry>> GetPurchaseTagsAsync(
            int offset = 0,
            int limit = 10,
            string name = null,
            Guid? purchaseId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default)
        {
            var resp = await _PurchaseTagStore.GetPurchaseTagsAsync(offset, limit, name, purchaseId, isActive, cancellationToken);

            return new Response<GetPurchaseTagEntry>()
            {
                Data = resp.Data.Select(x => new GetPurchaseTagEntry(x, true)),
                TotalPages = resp.TotalPages,
                Total = resp.Total
            };
        }

        public async Task<GetPurchaseTagEntry> AddPurchaseTagAsync(UpsertPurchaseTagEntry purchaseTag, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchaseTag, nameof(purchaseTag));

            var entry = purchaseTag.ToCommonEntry();
            var result = await _PurchaseTagStore.AddPurchaseTagAsync(entry, cancellationToken);

            return new GetPurchaseTagEntry(result, false);
        }

        public async Task DeletePurchaseTagAsync(Guid purchaseTagId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(purchaseTagId, nameof(purchaseTagId));

            await _PurchaseTagStore.DeletePurchaseTagAsync(purchaseTagId, cancellationToken);
        }

        public async Task<GetPurchaseTagEntry> UpdatePurchaseTagAsync(Guid purchaseTagId, UpsertPurchaseTagEntry purchaseTag, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchaseTag, nameof(purchaseTag));
            EnsureArg.IsNotDefault(purchaseTagId, nameof(purchaseTagId));

            var existingPurchaseTag = await _PurchaseTagStore.GetPurchaseTagAsync(purchaseTagId, cancellationToken);

            if (existingPurchaseTag is null)
            {
                throw new ResourceNotFoundException($"PurchaseTag Id '{purchaseTagId}' not found");
            }

            existingPurchaseTag.PatchFrom(purchaseTag);
            var result = await _PurchaseTagStore.UpdatePurchaseTagAsync(existingPurchaseTag, cancellationToken);

            return new GetPurchaseTagEntry(result, false);
        }
    }
}
