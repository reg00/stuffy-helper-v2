using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using StuffyHelper.Common.Exceptions;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Data.Repository.Interfaces;
using StuffyHelper.Data.Storage;

namespace StuffyHelper.Data.Repository
{
    public class DebtRepository : IDebtRepository
    {
        private readonly StuffyHelperContext _context;

        public DebtRepository(StuffyHelperContext context)
        {
            _context = context;
        }

        public async Task<DebtEntry> GetDebtAsync(Guid debtId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(debtId, nameof(debtId));

            try
            {
                var entry = await _context.Debts
                    .FirstOrDefaultAsync(e => e.Id == debtId,
                    cancellationToken);

                if (entry is null)
                    throw new EntityNotFoundException("Debt {DebtId} not found.", debtId);

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

        public async Task<DebtEntry?> GetDebtAsync(string lenderId, string debtorId, Guid eventId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNullOrWhiteSpace(lenderId, nameof(lenderId));
            EnsureArg.IsNotNullOrWhiteSpace(debtorId, nameof(debtorId));
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            try
            {
                return await _context.Debts
                    .FirstOrDefaultAsync(e => e.LenderId == lenderId && e.DebtorId == debtorId && e.EventId == eventId,
                    cancellationToken);
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

        public async Task<Response<DebtEntry>> GetDebtsAsync(
            int offset = 0,
            int limit = 10,
            string? lenderId = null,
            string? debtorId = null,
            bool? isSent = null,
            bool? isConfirmed = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var searchedData = await _context.Debts
                    .Where(e =>
                    (string.IsNullOrWhiteSpace(lenderId) || e.LenderId == lenderId) &&
                    (string.IsNullOrWhiteSpace(debtorId) || e.DebtorId == debtorId) &&
                    (isSent == null || isSent == e.IsSent) &&
                    (isConfirmed == null || isConfirmed == e.IsComfirmed))
                    .OrderByDescending(e => e.CreatedDate)
                    .ToListAsync(cancellationToken);

                return new Response<DebtEntry>()
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

        public async Task<Response<DebtEntry>> GetDebtsByUserAsync(
           string userId,
           int offset = 0,
           int limit = 10,
           CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNullOrWhiteSpace(userId, nameof(userId));

            try
            {
                var searchedData = await _context.Debts
                    .Where(e => e.DebtorId == userId || e.LenderId == userId)
                    .OrderByDescending(e => e.CreatedDate)
                    .ToListAsync(cancellationToken);

                return new Response<DebtEntry>()
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

        public async Task<DebtEntry> AddDebtAsync(DebtEntry debt, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(debt, nameof(debt));

            try
            {
                debt.CreatedDate = DateTime.UtcNow;
                var entry = await _context.Debts.AddAsync(debt, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return entry.Entity;
            }
            catch (DbUpdateException ex)
            {
                if ((ex.InnerException as PostgresException)?.SqlState == "23505")
                    throw new EntityAlreadyExistsException($"Debt already exists", ex);

                else throw new DbStoreException(ex);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        public async Task DeleteDebtAsync(Guid debtId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(debtId, nameof(debtId));

            try
            {
                var debt = await _context.Debts
                    .FirstOrDefaultAsync(
                    s => s.Id == debtId, cancellationToken);

                if (debt is null)
                {
                    throw new EntityNotFoundException("Debt {DebtId} not found.", debtId);
                }

                _context.Debts.Remove(debt);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        public async Task<DebtEntry> UpdateDebtAsync(DebtEntry debt, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(debt, nameof(debt));

            try
            {
                var entry = _context.Debts.Update(@debt);
                await _context.SaveChangesAsync(cancellationToken);
                return entry.Entity;
            }
            catch (DbUpdateException ex)
            {
                if ((ex.InnerException as PostgresException)?.SqlState == "23505")
                    throw new EntityAlreadyExistsException($"Debt already exists", ex);

                else throw new DbStoreException(ex);
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
