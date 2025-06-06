using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using StuffyHelper.Common.Exceptions;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Data.Repository.Interfaces;
using StuffyHelper.Data.Schema;

namespace StuffyHelper.Data.Repository
{
    public class CheckoutRepository : ICheckoutRepository
    {
        private readonly StuffyHelperContext _context;

        public CheckoutRepository(StuffyHelperContext context)
        {
            _context = context;
        }

        public async Task<CheckoutEntry> GetCheckoutAsync(Guid checkoutId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(checkoutId, nameof(checkoutId));

            try
            {
                var entry = await _context.Checkouts
                    .FirstOrDefaultAsync(e => e.Id == checkoutId,
                    cancellationToken);

                if (entry is null)
                    throw new EntityNotFoundException($"Checkout with Id '{checkoutId}' Not Found.");

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

        public async Task<Response<CheckoutEntry>> GetCheckoutsByEventIdAsync(Guid eventId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            try
            {
                var searchedData = await _context.Checkouts
                    .Where(e => eventId == e.EventId)
                    .OrderByDescending(e => e.CreatedDate)
                    .ToListAsync(cancellationToken);

                return new Response<CheckoutEntry>()
                {
                    Data = searchedData,
                    TotalPages = 1,
                    Total = searchedData.Count()
                };
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        public async Task<CheckoutEntry> AddCheckoutAsync(CheckoutEntry checkout, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(checkout, nameof(checkout));

            try
            {
                checkout.CreatedDate = DateTime.UtcNow;
                var entry = await _context.Checkouts.AddAsync(checkout, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return entry.Entity;
            }
            catch (DbUpdateException ex)
            {
                if ((ex.InnerException as PostgresException)?.SqlState == "23505")
                    throw new EntityAlreadyExistsException($"Checkout already exists", ex);

                else throw new DbStoreException(ex);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        public async Task DeleteCheckoutAsync(Guid checkoutId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(checkoutId, nameof(checkoutId));

            try
            {
                var checkout = await _context.Checkouts
                    .FirstOrDefaultAsync(
                    s => s.Id == checkoutId, cancellationToken);

                if (checkout is null)
                {
                    throw new EntityNotFoundException($"Checkout with Id '{checkoutId}' not found.");
                }

                _context.Checkouts.Remove(checkout);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }
    }
}
