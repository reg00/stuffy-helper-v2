using EnsureThat;
using Microsoft.EntityFrameworkCore;
using StuffyHelper.Common.Exceptions;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Data.Repository.Interfaces;
using StuffyHelper.Data.Schema;

namespace StuffyHelper.Data.Repository
{
    public class EfPurchaseTagRepository : IPurchaseTagRepository
    {
        private readonly StuffyHelperContext _context;

        public EfPurchaseTagRepository(StuffyHelperContext context)
        {
            _context = context;
        }

        public async Task<PurchaseTagEntry> GetPurchaseTagAsync(Guid purchaseTagId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(purchaseTagId, nameof(purchaseTagId));

            try
            {
                var entry = await _context.PurchaseTags
                    .FirstOrDefaultAsync(e => e.Id == purchaseTagId, cancellationToken);

                if (entry is null)
                    throw new EntityNotFoundException($"PurchaseTag with Id '{purchaseTagId}' Not Found.");

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

        public async Task<PurchaseTagEntry> GetPurchaseTagAsync(string name, CancellationToken cancellationToken = default)
        {
            try
            {
                var tag = await _context.PurchaseTags
                    .FirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower(), cancellationToken);

                if (tag is null)
                {
                    throw new EntityNotFoundException($"Purchase tag with name: {name} not found.");
                }

                return tag;
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

        public async Task<Response<PurchaseTagEntry>> GetPurchaseTagsAsync(
            int offset = 0,
            int limit = 10,
            string? name = null,
            Guid? purchaseId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var searchedData = await _context.PurchaseTags
                    .Where(e => (string.IsNullOrWhiteSpace(name) || e.Name.ToLower().Contains(name.ToLower())) &&
                    (isActive == null || isActive == e.IsActive) &&
                    (purchaseId == null || e.Purchases.Any(x => x.Id == purchaseId)))
                    .OrderByDescending(e => e.Name)
                    .ToListAsync(cancellationToken);

                return new Response<PurchaseTagEntry>()
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

        public async Task<PurchaseTagEntry> AddPurchaseTagAsync(PurchaseTagEntry purchaseTag, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchaseTag, nameof(purchaseTag));

            try
            {
                var entry = await _context.PurchaseTags.AddAsync(purchaseTag, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return entry.Entity;
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        public async Task DeletePurchaseTagAsync(Guid purchaseTagId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(purchaseTagId, nameof(purchaseTagId));

            try
            {
                var purchaseTag = await _context.PurchaseTags
                    .FirstOrDefaultAsync(
                    s => s.Id == purchaseTagId, cancellationToken);

                if (purchaseTag is null)
                {
                    throw new EntityNotFoundException($"PurchaseTag with Id '{purchaseTagId}' not found.");
                }

                purchaseTag.IsActive = false;

                _context.PurchaseTags.Update(purchaseTag);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        public async Task<PurchaseTagEntry> UpdatePurchaseTagAsync(PurchaseTagEntry purchaseTag, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchaseTag, nameof(purchaseTag));

            try
            {
                var entry = _context.PurchaseTags.Update(purchaseTag);
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
