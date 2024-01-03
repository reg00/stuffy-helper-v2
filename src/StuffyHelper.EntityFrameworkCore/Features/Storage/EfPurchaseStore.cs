using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Reg00.Infrastructure.Errors;
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
                    .Include(e => e.Event)
                    .Include(e => e.PurchaseUsages)
                    .Include(e => e.UnitType)
                    .Include(e => e.PurchaseTags)
                    .FirstOrDefaultAsync(e => e.Id == purchaseId, cancellationToken);

                if (entry is null)
                    throw new EntityNotFoundException($"Purchase with Id '{purchaseId}' Not Found.");

                entry.PurchaseTags = entry.PurchaseTags.Where(x => x.IsActive == true).ToList();

                return entry;
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }

        }

        public async Task<PagedData<PurchaseEntry>> GetPurchasesAsync(
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
            try
            {
                var searchedData = await _context.Purchases
                    .Include(e => e.PurchaseTags)
                    .Include(e => e.UnitType)
                    .Where(e => (string.IsNullOrWhiteSpace(name) || e.Name.ToLower().Contains(name.ToLower())) &&
                    (costMin == null || costMin <= e.Cost) &&
                    (costMax == null || costMax >= e.Cost) &&
                    (eventId == null || e.EventId == eventId) &&
                    (unitTypeId == null || e.UnitTypeId == unitTypeId) &&
                    (isComplete == null || e.IsComplete == isComplete) &&
                    (purchaseTags == null || !purchaseTags.Any() || e.PurchaseTags.Any(tag => purchaseTags.Select(tag => tag.ToLower()).Contains(tag.Name.ToLower()))))
                    .OrderByDescending(e => e.Event.CreatedDate)
                    .ToListAsync(cancellationToken);

                return new PagedData<PurchaseEntry>()
                {
                    Data = searchedData.Skip(offset).Take(limit),
                    TotalPages = (int)Math.Ceiling(searchedData.Count() / (double)limit),
                    Total = searchedData.Count()
                };
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        public async Task<PurchaseEntry> AddPurchaseAsync(PurchaseEntry purchase, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchase, nameof(purchase));

            try
            {
                var entry = await _context.Purchases.AddAsync(purchase, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                _context.Entry(entry.Entity).Reference(x => x.UnitType).Load();
                entry.Entity.PurchaseTags = entry.Entity.PurchaseTags.Where(x => x.IsActive == true).ToList();
                return entry.Entity;
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
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
                    throw new EntityNotFoundException($"Purchase with Id '{purchaseId}' not found.");
                }

                _context.Purchases.Remove(purchase);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        public async Task CompletePurchaseAsync(Guid purchaseId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(purchaseId, nameof(purchaseId));

            try
            {
                var purchase = await _context.Purchases
                    .FirstOrDefaultAsync(
                    s => s.Id == purchaseId, cancellationToken);

                if (purchase is null)
                {
                    throw new EntityNotFoundException($"Purchase with Id '{purchaseId}' not found.");
                }

                purchase.IsComplete = true;
                _context.Purchases.Update(purchase);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        public async Task<PurchaseEntry> UpdatePurchaseAsync(PurchaseEntry purchase, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchase, nameof(purchase));

            try
            {
                var entry = _context.Purchases.Update(purchase);
                await _context.SaveChangesAsync(cancellationToken);
                entry.Entity.PurchaseTags = entry.Entity.PurchaseTags.Where(x => x.IsActive == true).ToList();
                return entry.Entity;
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }
    }
}
