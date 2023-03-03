using EnsureThat;
using Reg00.Infrastructure.Errors;
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

        public async Task<Response<GetPurchaseEntry>> GetPurchasesAsync(
            int offset = 0,
            int limit = 10,
            string? name = null,
            double? costMin = null,
            double? costMax = null,
            Guid? eventId = null,
            IEnumerable<string>? purchaseTags = null,
            Guid? unitTypeId = null,
            bool? isComplete = null,
            CancellationToken cancellationToken = default)
        {
            var resp = await _purchaseStore.GetPurchasesAsync(offset, limit, name, costMin, costMax,
                                                              eventId, purchaseTags, unitTypeId, isComplete, cancellationToken);

            return new Response<GetPurchaseEntry>()
            {
                Data = resp.Data.Select(x => new GetPurchaseEntry(x)),
                TotalPages = resp.TotalPages,
                Total = resp.Total
            };
        }

        public async Task<PurchaseShortEntry> AddPurchaseAsync(AddPurchaseEntry purchase, CancellationToken cancellationToken = default)
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

        public async Task CompletePurchaseAsync(Guid purchaseId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(purchaseId, nameof(purchaseId));

            await _purchaseStore.CompletePurchaseAsync(purchaseId, cancellationToken);
        }

        public async Task<PurchaseShortEntry> UpdatePurchaseAsync(Guid purchaseId, UpdatePurchaseEntry purchase, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchase, nameof(purchase));
            EnsureArg.IsNotDefault(purchaseId, nameof(purchaseId));

            var existingPurchase = await _purchaseStore.GetPurchaseAsync(purchaseId, cancellationToken);

            if (existingPurchase is null)
            {
                throw new EntityNotFoundException($"Purchase Id '{purchaseId}' not found");
            }

            existingPurchase.PatchFrom(purchase);
            await _purchaseTagPipeline.ProcessAsync(existingPurchase, purchase.PurchaseTags, cancellationToken);
            var result = await _purchaseStore.UpdatePurchaseAsync(existingPurchase, cancellationToken);

            return new PurchaseShortEntry(result);
        }
    }
}
