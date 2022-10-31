using EnsureThat;
using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.PurchaseTag.Pipeline;

namespace StuffyHelper.Core.Features.Purchase
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseStore _purchaseStore;
        private readonly IPurchaseTagPipeline _purchaseTagPipeline;

        public PurchaseService(
            IPurchaseStore purchaseStore,
            IPurchaseTagPipeline purchaseTagPipeline)
        {
            _purchaseStore = purchaseStore;
            _purchaseTagPipeline = purchaseTagPipeline;
        }

        public async Task<GetPurchaseEntry> GetPurchaseAsync(Guid purchaseId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(purchaseId, nameof(purchaseId));

            var entry = await _purchaseStore.GetPurchaseAsync(purchaseId, cancellationToken);

            return new GetPurchaseEntry(entry);
        }

        public async Task<Response<PurchaseShortEntry>> GetPurchasesAsync(
            int offset = 0,
            int limit = 10,
            string name = null,
            double? costMin = null,
            double? costMax = null,
            Guid? shoppingId = null,
            IEnumerable<string> purchaseTags = null,
            Guid? unitTypeId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default)
        {
            var resp = await _purchaseStore.GetPurchasesAsync(offset, limit, name, costMin, costMax,
                                                              shoppingId, purchaseTags, unitTypeId, isActive, cancellationToken);

            return new Response<PurchaseShortEntry>()
            {
                Data = resp.Data.Select(x => new PurchaseShortEntry(x)),
                TotalPages = resp.TotalPages,
                Total = resp.Total
            };
        }

        public async Task<PurchaseShortEntry> AddPurchaseAsync(UpsertPurchaseEntry purchase, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchase, nameof(purchase));

            var entry = purchase.ToCommonEntry();
            await _purchaseTagPipeline.ProcessAsync(entry, purchase.PurchaseTags, cancellationToken);
            var result = await _purchaseStore.AddPurchaseAsync(entry, cancellationToken);

            return new PurchaseShortEntry(result);
        }

        public async Task DeletePurchaseAsync(Guid purchaseId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(purchaseId, nameof(purchaseId));

            await _purchaseStore.DeletePurchaseAsync(purchaseId, cancellationToken);
        }

        public async Task<PurchaseShortEntry> UpdatePurchaseAsync(Guid purchaseId, UpsertPurchaseEntry purchase, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchase, nameof(purchase));
            EnsureArg.IsNotDefault(purchaseId, nameof(purchaseId));

            var existingPurchase = await _purchaseStore.GetPurchaseAsync(purchaseId, cancellationToken);

            if (existingPurchase is null)
            {
                throw new ResourceNotFoundException($"Purchase Id '{purchaseId}' not found");
            }

            existingPurchase.PatchFrom(purchase);
            await _purchaseTagPipeline.ProcessAsync(existingPurchase, purchase.PurchaseTags, cancellationToken);
            var result = await _purchaseStore.UpdatePurchaseAsync(existingPurchase, cancellationToken);

            return new PurchaseShortEntry(result);
        }
    }
}
