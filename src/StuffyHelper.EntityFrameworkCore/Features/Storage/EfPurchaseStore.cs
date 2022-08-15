using EnsureThat;
using Microsoft.EntityFrameworkCore;
using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Purchase;
using StuffyHelper.EntityFrameworkCore.Features.Schema;

namespace StuffyHelper.EntityFrameworkCore.Features.Storage
{
    public class EfPurchaseStore : IPurchaseStore
    {
        private readonly StuffyHelperContext _context;

        public EfPurchaseStore(StuffyHelperContext context)
        {
            _context = context;
        }

        public async Task<PurchaseEntry> GetPurchaseAsync(Guid purchaseId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(purchaseId, nameof(purchaseId));

            try
            {
                var entry = await _context.Purchases
                    .FirstOrDefaultAsync(e => e.Id == purchaseId, cancellationToken);

                if (entry is null)
                    throw new ResourceNotFoundException($"Purchase with Id '{purchaseId}' Not Found.");

                return entry;
            }
            catch (ResourceNotFoundException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }

        }

        public async Task<Response<PurchaseEntry>> GetPurchasesAsync(
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
            try
            {
                var searchedData = await _context.Purchases
                    .Where(e => (string.IsNullOrWhiteSpace(name) || e.Name.Contains(name, StringComparison.OrdinalIgnoreCase)) &&
                    (countMin == null || countMin <= e.Count) &&
                    (countMax == null || countMax >= e.Count) &&
                    (amountMin == null || amountMin <= e.Amount) &&
                    (amountMax == null || amountMax >= e.Amount) &&
                    (isActive == null || isActive == e.IsActive) &&
                    (shoppingId == null || e.ShoppingId == shoppingId))
                    .OrderByDescending(e => e.Shopping.Event.CreatedDate)
                    .ToListAsync(cancellationToken);

                return new Response<PurchaseEntry>()
                {
                    Data = searchedData.Skip(offset).Take(limit),
                    TotalPages = (int)Math.Ceiling(searchedData.Count() / (double)limit),
                    Total = searchedData.Count()
                };
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        public async Task<PurchaseEntry> AddPurchaseAsync(PurchaseEntry purchase, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchase, nameof(purchase));

            try
            {
                var entry = await _context.Purchases.AddAsync(purchase, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return entry.Entity;
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        public async Task DeletePurchaseAsync(Guid purchaseId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(purchaseId, nameof(purchaseId));

            try
            {
                var purchase = await _context.Purchases
                    .FirstOrDefaultAsync(
                    s => s.Id == purchaseId, cancellationToken);

                if (purchase is null)
                {
                    throw new ResourceNotFoundException($"Purchase with Id '{purchaseId}' not found.");
                }

                purchase.IsActive = false;

                _context.Purchases.Update(purchase);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        public async Task<PurchaseEntry> UpdatePurchaseAsync(PurchaseEntry purchase, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchase, nameof(purchase));

            try
            {
                var entry = _context.Purchases.Update(purchase);
                await _context.SaveChangesAsync(cancellationToken);
                return entry.Entity;
            }
            catch (ResourceNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }
    }
}
