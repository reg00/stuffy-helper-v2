using EnsureThat;
using Reg00.Infrastructure.Errors;
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

            return new GetPurchaseTagEntry(entry);
        }

        public async Task<Response<PurchaseTagShortEntry>> GetPurchaseTagsAsync(
            int offset = 0,
            int limit = 10,
            string? name = null,
            Guid? purchaseId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default)
        {
            var resp = await _PurchaseTagStore.GetPurchaseTagsAsync(offset, limit, name, purchaseId, isActive, cancellationToken);

            return new Response<PurchaseTagShortEntry>()
            {
                Data = resp.Data.Select(x => new PurchaseTagShortEntry(x)),
                TotalPages = resp.TotalPages,
                Total = resp.Total
            };
        }

        public async Task<PurchaseTagShortEntry> AddPurchaseTagAsync(UpsertPurchaseTagEntry purchaseTag, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchaseTag, nameof(purchaseTag));

            var entry = purchaseTag.ToCommonEntry();
            var result = await _PurchaseTagStore.AddPurchaseTagAsync(entry, cancellationToken);

            return new PurchaseTagShortEntry(result);
        }

        public async Task DeletePurchaseTagAsync(Guid purchaseTagId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(purchaseTagId, nameof(purchaseTagId));

            await _PurchaseTagStore.DeletePurchaseTagAsync(purchaseTagId, cancellationToken);
        }

        public async Task<PurchaseTagShortEntry> UpdatePurchaseTagAsync(Guid purchaseTagId, UpsertPurchaseTagEntry purchaseTag, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchaseTag, nameof(purchaseTag));
            EnsureArg.IsNotDefault(purchaseTagId, nameof(purchaseTagId));

            var existingPurchaseTag = await _PurchaseTagStore.GetPurchaseTagAsync(purchaseTagId, cancellationToken);

            if (existingPurchaseTag is null)
            {
                throw new EntityNotFoundException($"PurchaseTag Id '{purchaseTagId}' not found");
            }

            existingPurchaseTag.PatchFrom(purchaseTag);
            var result = await _PurchaseTagStore.UpdatePurchaseTagAsync(existingPurchaseTag, cancellationToken);

            return new PurchaseTagShortEntry(result);
        }
    }
}
