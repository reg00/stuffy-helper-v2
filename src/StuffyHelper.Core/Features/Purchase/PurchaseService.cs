using EnsureThat;
using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.Purchase
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseStore _purchaseStore;

        public PurchaseService(IPurchaseStore purchaseStore)
        {
            _purchaseStore = purchaseStore;
        }

        public async Task<GetPurchaseEntry> GetPurchaseAsync(Guid purchaseId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(purchaseId, nameof(purchaseId));

            var entry = await _purchaseStore.GetPurchaseAsync(purchaseId, cancellationToken);

            return new GetPurchaseEntry(entry, true , true);
        }

        public async Task<Response<GetPurchaseEntry>> GetPurchasesAsync(
            int offset = 0,
            int limit = 10,
            string name = null,
            int? countMin = null,
            int? countMax = null,
            double? amountMin = null,
            double? amountMax = null,
            Guid? shoppingId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default)
        {
            var resp = await _purchaseStore.GetPurchasesAsync(offset, limit, name, countMin, countMax, amountMin, amountMax, shoppingId, isActive, cancellationToken);

            return new Response<GetPurchaseEntry>()
            {
                Data = resp.Data.Select(x => new GetPurchaseEntry(x, true, true)),
                TotalPages = resp.TotalPages
            };
        }

        public async Task<GetPurchaseEntry> AddPurchaseAsync(UpsertPurchaseEntry purchase, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchase, nameof(purchase));

            var entry = purchase.ToCommonEntry();
            var result = await _purchaseStore.AddPurchaseAsync(entry, cancellationToken);

            return new GetPurchaseEntry(result, false, false);
        }

        public async Task DeletePurchaseAsync(Guid purchaseId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(purchaseId, nameof(purchaseId));

            await _purchaseStore.DeletePurchaseAsync(purchaseId, cancellationToken);
        }

        public async Task<GetPurchaseEntry> UpdatePurchaseAsync(Guid purchaseId, UpsertPurchaseEntry purchase, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchase, nameof(purchase));
            EnsureArg.IsNotDefault(purchaseId, nameof(purchaseId));

            var existingPurchase = await _purchaseStore.GetPurchaseAsync(purchaseId, cancellationToken);

            if (existingPurchase is null)
            {
                throw new ResourceNotFoundException($"Purchase Id '{purchaseId}' not found");
            }

            existingPurchase.PatchFrom(purchase);
            var result = await _purchaseStore.UpdatePurchaseAsync(existingPurchase, cancellationToken);

            return new GetPurchaseEntry(result, false, false);
        }
    }
}
