using EnsureThat;
using Microsoft.EntityFrameworkCore;
using StuffyHelper.Common.Exceptions;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Data.Repository.Interfaces;
using StuffyHelper.Data.Schema;

namespace StuffyHelper.Data.Repository
{
    public class PurchaseUsageRepository : IPurchaseUsageRepository
    {
        private readonly StuffyHelperContext _context;

        public PurchaseUsageRepository(StuffyHelperContext context)
        {
            _context = context;
        }

        public async Task<PurchaseUsageEntry> GetPurchaseUsageAsync(Guid purchaseUsageId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(purchaseUsageId, nameof(purchaseUsageId));

            try
            {
                var entry = await _context.PurchaseUsages
                    .FirstOrDefaultAsync(e => e.Id == purchaseUsageId, cancellationToken);

                if (entry is null)
                    throw new EntityNotFoundException($"PurchaseUsage with Id '{purchaseUsageId}' Not Found.");

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

        public async Task<Response<PurchaseUsageEntry>> GetPurchaseUsagesAsync(
            int offset = 0,
            int limit = 10,
            Guid? eventId = null,
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
                    (eventId == null || eventId == e.Purchase.EventId))
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

        public async Task DeletePurchaseUsageAsync(Guid purchaseUsageId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(purchaseUsageId, nameof(purchaseUsageId));

            try
            {
                var PurchaseUsage = await _context.PurchaseUsages
                    .FirstOrDefaultAsync(
                    s => s.Id == purchaseUsageId, cancellationToken);

                if (PurchaseUsage is null)
                {
                    throw new EntityNotFoundException($"PurchaseUsage with Id '{purchaseUsageId}' not found.");
                }

                //PurchaseUsage.IsActive = false;

                _context.PurchaseUsages.Remove(PurchaseUsage);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

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
