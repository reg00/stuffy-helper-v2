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
    public class PurchaseUsageRepository : IPurchaseUsageRepository
    {
        private readonly StuffyHelperContext _context;

        /// <summary>
        /// Ctor.
        /// </summary>
        public PurchaseUsageRepository(StuffyHelperContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<PurchaseUsageEntry> GetPurchaseUsageAsync(Guid eventId, Guid purchaseUsageId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(purchaseUsageId, nameof(purchaseUsageId));
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            try
            {
                var entry = await _context.PurchaseUsages.Include(x => x.Purchase)
                    .FirstOrDefaultAsync(e => e.Id == purchaseUsageId && e.Purchase.EventId == eventId, cancellationToken);

                if (entry is null)
                    throw new EntityNotFoundException("Purchase Usage {PurchaseUsageId} not found.", purchaseUsageId);

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
        public async Task<Response<PurchaseUsageEntry>> GetPurchaseUsagesAsync(
            Guid eventId,
            int offset = 0,
            int limit = 10,
            Guid? participantId = null,
            Guid? purchaseId = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var searchedData = await _context.PurchaseUsages
                    .Include(x => x.Purchase)
                    .Include(x => x.Participant)
                    .Where(e => (participantId == null || participantId == e.ParticipantId) &&
                    (purchaseId == null || purchaseId == e.PurchaseId) &&
                    eventId == e.Purchase.EventId)
                    .OrderByDescending(e => e.Participant.Event.CreatedDate)
                    .ToListAsync(cancellationToken);

                return new Response<PurchaseUsageEntry>()
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
        public async Task<PurchaseUsageEntry> AddPurchaseUsageAsync(PurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchaseUsage, nameof(purchaseUsage));

            try
            {
                var entry = await _context.PurchaseUsages.AddAsync(purchaseUsage, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return entry.Entity;
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        /// <inheritdoc />
        public async Task DeletePurchaseUsageAsync(Guid eventId, Guid purchaseUsageId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(purchaseUsageId, nameof(purchaseUsageId));
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            try
            {
                var purchaseUsage = await _context.PurchaseUsages.Include(x => x.Purchase)
                    .FirstOrDefaultAsync(
                    s => s.Id == purchaseUsageId && s.Purchase.EventId == eventId, cancellationToken);

                if (purchaseUsage is null)
                {
                    throw new EntityNotFoundException("Purchase Usage {PurchaseUsageId} not found.", purchaseUsageId);
                }

                //PurchaseUsage.IsActive = false;

                _context.PurchaseUsages.Remove(purchaseUsage);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        public async Task DeleteEventPurchaseUsages(Guid eventId, CancellationToken cancellationToken = default)
        {
            try
            {
                var purchaseUsages = await _context.PurchaseUsages
                    .Include(x => x.Purchase)
                    .Where(s => s.Purchase.EventId == eventId).ToListAsync(cancellationToken);

                if (purchaseUsages.Any())
                {
                    _context.PurchaseUsages.RemoveRange(purchaseUsages);
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        /// <inheritdoc />
        public async Task<PurchaseUsageEntry> UpdatePurchaseUsageAsync(PurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchaseUsage, nameof(purchaseUsage));

            try
            {
                var entry = _context.PurchaseUsages.Update(purchaseUsage);
                await _context.SaveChangesAsync(cancellationToken);
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
