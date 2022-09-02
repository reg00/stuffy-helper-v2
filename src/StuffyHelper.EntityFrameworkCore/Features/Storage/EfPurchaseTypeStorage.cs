using EnsureThat;
using Microsoft.EntityFrameworkCore;
using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.PurchaseType;
using StuffyHelper.EntityFrameworkCore.Features.Schema;

namespace StuffyHelper.EntityFrameworkCore.Features.Storage
{
    public class EfPurchaseTypeStore : IPurchaseTypeStore
    {
        private readonly StuffyHelperContext _context;

        public EfPurchaseTypeStore(StuffyHelperContext context)
        {
            _context = context;
        }

        public async Task<PurchaseTypeEntry> GetPurchaseTypeAsync(Guid purchaseTypeId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(purchaseTypeId, nameof(purchaseTypeId));

            try
            {
                var entry = await _context.PurchaseTypes
                    .FirstOrDefaultAsync(e => e.Id == purchaseTypeId, cancellationToken);

                if (entry is null)
                    throw new ResourceNotFoundException($"PurchaseType with Id '{purchaseTypeId}' Not Found.");

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

        public async Task<Response<PurchaseTypeEntry>> GetPurchaseTypesAsync(
            int offset = 0,
            int limit = 10,
            string name = null,
            Guid? purchaseId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var searchedData = await _context.PurchaseTypes
                    .Where(e => (string.IsNullOrWhiteSpace(name) || e.Name.ToLower().Contains(name.ToLower())) &&
                    (isActive == null || isActive == e.IsActive) &&
                    (purchaseId == null || e.Purchases.Any(x => x.Id == purchaseId)))
                    .OrderByDescending(e => e.Name)
                    .ToListAsync(cancellationToken);

                return new Response<PurchaseTypeEntry>()
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

        public async Task<PurchaseTypeEntry> AddPurchaseTypeAsync(PurchaseTypeEntry purchaseType, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchaseType, nameof(purchaseType));

            try
            {
                var entry = await _context.PurchaseTypes.AddAsync(purchaseType, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return entry.Entity;
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        public async Task DeletePurchaseTypeAsync(Guid purchaseTypeId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(purchaseTypeId, nameof(purchaseTypeId));

            try
            {
                var purchaseType = await _context.PurchaseTypes
                    .FirstOrDefaultAsync(
                    s => s.Id == purchaseTypeId, cancellationToken);

                if (purchaseType is null)
                {
                    throw new ResourceNotFoundException($"PurchaseType with Id '{purchaseTypeId}' not found.");
                }

                purchaseType.IsActive = false;

                _context.PurchaseTypes.Update(purchaseType);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        public async Task<PurchaseTypeEntry> UpdatePurchaseTypeAsync(PurchaseTypeEntry purchaseType, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchaseType, nameof(purchaseType));

            try
            {
                var entry = _context.PurchaseTypes.Update(purchaseType);
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
