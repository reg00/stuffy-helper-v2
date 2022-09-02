using EnsureThat;
using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.PurchaseType
{
    public class PurchaseTypeService : IPurchaseTypeService
    {
        private readonly IPurchaseTypeStore _PurchaseTypeStore;

        public PurchaseTypeService(IPurchaseTypeStore PurchaseTypeStore)
        {
            _PurchaseTypeStore = PurchaseTypeStore;
        }

        public async Task<GetPurchaseTypeEntry> GetPurchaseTypeAsync(Guid purchaseTypeId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(purchaseTypeId, nameof(purchaseTypeId));

            var entry = await _PurchaseTypeStore.GetPurchaseTypeAsync(purchaseTypeId, cancellationToken);

            return new GetPurchaseTypeEntry(entry, true);
        }

        public async Task<Response<GetPurchaseTypeEntry>> GetPurchaseTypesAsync(
            int offset = 0,
            int limit = 10,
            string name = null,
            Guid? purchaseId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default)
        {
            var resp = await _PurchaseTypeStore.GetPurchaseTypesAsync(offset, limit, name, purchaseId, isActive, cancellationToken);

            return new Response<GetPurchaseTypeEntry>()
            {
                Data = resp.Data.Select(x => new GetPurchaseTypeEntry(x, true)),
                TotalPages = resp.TotalPages,
                Total = resp.Total
            };
        }

        public async Task<GetPurchaseTypeEntry> AddPurchaseTypeAsync(UpsertPurchaseTypeEntry purchaseType, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchaseType, nameof(purchaseType));

            var entry = purchaseType.ToCommonEntry();
            var result = await _PurchaseTypeStore.AddPurchaseTypeAsync(entry, cancellationToken);

            return new GetPurchaseTypeEntry(result, false);
        }

        public async Task DeletePurchaseTypeAsync(Guid purchaseTypeId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(purchaseTypeId, nameof(purchaseTypeId));

            await _PurchaseTypeStore.DeletePurchaseTypeAsync(purchaseTypeId, cancellationToken);
        }

        public async Task<GetPurchaseTypeEntry> UpdatePurchaseTypeAsync(Guid purchaseTypeId, UpsertPurchaseTypeEntry purchaseType, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchaseType, nameof(purchaseType));
            EnsureArg.IsNotDefault(purchaseTypeId, nameof(purchaseTypeId));

            var existingPurchaseType = await _PurchaseTypeStore.GetPurchaseTypeAsync(purchaseTypeId, cancellationToken);

            if (existingPurchaseType is null)
            {
                throw new ResourceNotFoundException($"PurchaseType Id '{purchaseTypeId}' not found");
            }

            existingPurchaseType.PatchFrom(purchaseType);
            var result = await _PurchaseTypeStore.UpdatePurchaseTypeAsync(existingPurchaseType, cancellationToken);

            return new GetPurchaseTypeEntry(result, false);
        }
    }
}
