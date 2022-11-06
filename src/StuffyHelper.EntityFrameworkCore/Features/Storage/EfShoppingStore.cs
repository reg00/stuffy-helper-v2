using EnsureThat;
using Microsoft.EntityFrameworkCore;
using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Shopping;
using StuffyHelper.EntityFrameworkCore.Features.Schema;

namespace StuffyHelper.EntityFrameworkCore.Features.Storage
{
    public class EfShoppingStore : IShoppingStore
    {
        private readonly StuffyHelperContext _context;

        public EfShoppingStore(StuffyHelperContext context)
        {
            _context = context;
        }

        public async Task<ShoppingEntry> GetShoppingAsync(Guid shoppingId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(shoppingId, nameof(shoppingId));

            try
            {
                var entry = await _context.Shoppings
                    .FirstOrDefaultAsync(e => e.Id == shoppingId, cancellationToken);

                if (entry is null)
                    throw new ResourceNotFoundException($"Shopping with Id '{shoppingId}' Not Found.");

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

        public async Task<Response<ShoppingEntry>> GetShoppingsAsync(
            int offset = 0,
            int limit = 10,
            DateTime? shoppingDateStart = null,
            DateTime? shoppingDateEnd = null,
            Guid? participantId = null,
            Guid? eventId = null,
            string description = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var searchedData = await _context.Shoppings
                    .Where(e => (shoppingDateStart == null || e.ShoppingDate >= shoppingDateStart.Value) &&
                    (shoppingDateEnd == null || e.ShoppingDate <= shoppingDateEnd.Value) &&
                    (participantId == null || e.ParticipantId == participantId) &&
                    (eventId == null || e.EventId == eventId) &&
                    (string.IsNullOrEmpty(description) || e.Description.ToLower().Contains(description.ToLower())))
                    .OrderByDescending(e => e.Event.CreatedDate)
                    .ToListAsync(cancellationToken);

                return new Response<ShoppingEntry>()
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

        public async Task<ShoppingEntry> AddShoppingAsync(ShoppingEntry shopping, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(shopping, nameof(shopping));

            try
            {
                var entry = await _context.Shoppings.AddAsync(shopping, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return entry.Entity;
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        public async Task DeleteShoppingAsync(Guid shoppingId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(shoppingId, nameof(shoppingId));

            try
            {
                var Shopping = await _context.Shoppings
                    .FirstOrDefaultAsync(
                    s => s.Id == shoppingId, cancellationToken);

                if (Shopping is null)
                {
                    throw new ResourceNotFoundException($"Shopping with Id '{shoppingId}' not found.");
                }

                _context.Shoppings.Remove(Shopping);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        public async Task<ShoppingEntry> UpdateShoppingAsync(ShoppingEntry shopping, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(shopping, nameof(shopping));

            try
            {
                var entry = _context.Shoppings.Update(shopping);
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
