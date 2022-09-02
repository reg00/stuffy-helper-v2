using EnsureThat;
using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.PurchaseUsage
{
    public class PurchaseUsageService : IPurchaseUsageService
    {
        private readonly IPurchaseUsageStore _purchaseUsageStore;

        public PurchaseUsageService(IPurchaseUsageStore purchaseUsageStore)
        {
            _purchaseUsageStore = purchaseUsageStore;
        }

        public async Task<GetPurchaseUsageEntry> GetPurchaseUsageAsync(Guid purchaseUsageId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(purchaseUsageId, nameof(purchaseUsageId));

            var entry = await _purchaseUsageStore.GetPurchaseUsageAsync(purchaseUsageId, cancellationToken);

            return new GetPurchaseUsageEntry(entry, true, true);
        }

        public async Task<Response<GetPurchaseUsageEntry>> GetPurchaseUsagesAsync(
            int offset = 0,
            int limit = 10,
            Guid? participantId = null,
            Guid? purchaseId = null,
            CancellationToken cancellationToken = default)
        {
            var resp = await _purchaseUsageStore.GetPurchaseUsagesAsync(offset, limit, participantId, purchaseId, cancellationToken);

            return new Response<GetPurchaseUsageEntry>()
            {
                Data = resp.Data.Select(x => new GetPurchaseUsageEntry(x, true ,true)),
                TotalPages = resp.TotalPages,
                Total = resp.Total
            };
        }

        public async Task<GetPurchaseUsageEntry> AddPurchaseUsageAsync(UpsertPurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchaseUsage, nameof(purchaseUsage));

            var entry = purchaseUsage.ToCommonEntry();
            var result = await _purchaseUsageStore.AddPurchaseUsageAsync(entry, cancellationToken);

            return new GetPurchaseUsageEntry(result, false, false);
        }

        public async Task DeletePurchaseUsageAsync(Guid purchaseUsageId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(purchaseUsageId, nameof(purchaseUsageId));

            await _purchaseUsageStore.DeletePurchaseUsageAsync(purchaseUsageId, cancellationToken);
        }

        public async Task<GetPurchaseUsageEntry> UpdatePurchaseUsageAsync(Guid purchaseUsageId, UpsertPurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchaseUsage, nameof(purchaseUsage));
            EnsureArg.IsNotDefault(purchaseUsageId, nameof(purchaseUsageId));

            var existingPurchaseUsage = await _purchaseUsageStore.GetPurchaseUsageAsync(purchaseUsageId, cancellationToken);

            if (existingPurchaseUsage is null)
            {
                throw new ResourceNotFoundException($"PurchaseUsage Id '{purchaseUsageId}' not found");
            }

            existingPurchaseUsage.PatchFrom(purchaseUsage);
            var result = await _purchaseUsageStore.UpdatePurchaseUsageAsync(existingPurchaseUsage, cancellationToken);

            return new GetPurchaseUsageEntry(result, false, false);
        }
    }
}
