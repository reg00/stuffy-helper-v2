using EnsureThat;
using Microsoft.EntityFrameworkCore;
using StuffyHelper.Common.Exceptions;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Data.Repository.Interfaces;
using StuffyHelper.Data.Storage;

namespace StuffyHelper.Data.Repository
{
    /// <inheritdoc />
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly StuffyHelperContext _context;

        /// <summary>
        /// Ctor.
        /// </summary>
        public PurchaseRepository(StuffyHelperContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<PurchaseEntry> GetPurchaseAsync(Guid eventId, Guid purchaseId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(purchaseId, nameof(purchaseId));
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            try
            {
                var entry = await _context.Purchases
                    .Include(e => e.Event)
                    .Include(e => e.PurchaseUsages)
                    .Include(e => e.UnitType)
                    .Include(e => e.PurchaseTags)
                    .FirstOrDefaultAsync(e => e.Id == purchaseId && e.EventId == eventId, cancellationToken);

                if (entry is null)
                    throw new EntityNotFoundException("Purchase {PurchaseId} not found.", purchaseId);
                
                entry.PurchaseTags = entry.PurchaseTags.Where(x => x.IsActive).ToList();

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

        /// <inheritdoc />
        public async Task<Response<PurchaseEntry>> GetPurchasesAsync(
            Guid eventId,
            int offset = 0,
            int limit = 10,
            string? name = null,
            long? costMin = null,
            long? costMax = null,
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
                    e.EventId == eventId &&
                    (costMin == null || costMin <= e.Cost) &&
                    (costMax == null || costMax >= e.Cost) &&
                    (unitTypeId == null || e.UnitTypeId == unitTypeId) &&
                    (isComplete == null || e.IsComplete == isComplete) &&
                    (purchaseTags == null || !purchaseTags.Any() || e.PurchaseTags.Any(tag => purchaseTags.Select(s => s.ToLower()).Contains(tag.Name.ToLower()))))
                    .OrderByDescending(e => e.Event.CreatedDate)
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
                throw new DbStoreException(ex);
            }
        }

        /// <inheritdoc />
        public async Task<PurchaseEntry> AddPurchaseAsync(PurchaseEntry purchase, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchase, nameof(purchase));

            try
            {
                var entry = await _context.Purchases.AddAsync(purchase, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.Entry(entry.Entity).Reference(x => x.UnitType).LoadAsync(cancellationToken);
                entry.Entity.PurchaseTags = entry.Entity.PurchaseTags.Where(x => x.IsActive).ToList();
                return entry.Entity;
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        /// <inheritdoc />
        public async Task DeletePurchaseAsync(Guid eventId, Guid purchaseId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(purchaseId, nameof(purchaseId));
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            try
            {
                var purchase = await _context.Purchases
                    .FirstOrDefaultAsync(
                    s => s.Id == purchaseId && s.EventId == eventId, cancellationToken);

                if (purchase is null)
                {
                    throw new EntityNotFoundException("Purchase {PurchaseId} not found.", purchaseId);
                }

                _context.Purchases.Remove(purchase);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        /// <inheritdoc />
        public async Task CompletePurchaseAsync(Guid eventId, Guid purchaseId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(purchaseId, nameof(purchaseId));
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            try
            {
                var purchase = await _context.Purchases
                    .FirstOrDefaultAsync(
                    s => s.Id == purchaseId && eventId == s.EventId, cancellationToken);

                if (purchase is null)
                {
                    throw new EntityNotFoundException("Purchase {PurchaseId} not found.", purchaseId);
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

        /// <inheritdoc />
        public async Task<PurchaseEntry> UpdatePurchaseAsync(PurchaseEntry purchase, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchase, nameof(purchase));

            try
            {
                var entry = _context.Purchases.Update(purchase);
                await _context.SaveChangesAsync(cancellationToken);
                entry.Entity.PurchaseTags = entry.Entity.PurchaseTags.Where(x => x.IsActive).ToList();
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
