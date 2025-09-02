using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Data.Repository.Interfaces
{
    /// <summary>
    /// Interface for work with debt repository
    /// </summary>
    public interface IDebtRepository
    {
        /// <summary>
        /// Get debt by identifier
        /// </summary>
        /// <param name="debtId">Debt identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Debt entry</returns>
        Task<DebtEntry> GetDebtAsync(Guid debtId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get debt by lender, debtor and event identifiers
        /// </summary>
        /// <param name="lenderId">Lender user identifier</param>
        /// <param name="debtorId">Debtor user identifier</param>
        /// <param name="eventId">Event identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Debt entry or null if not found</returns>
        Task<DebtEntry?> GetDebtAsync(string lenderId, string debtorId, Guid eventId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get filtered list of debts
        /// </summary>
        /// <param name="offset">Number of records to skip</param>
        /// <param name="limit">Maximum number of records to return</param>
        /// <param name="lenderId">Lender user identifier filter</param>
        /// <param name="debtorId">Debtor user identifier filter</param>
        /// <param name="isSent">Debt sent status filter</param>
        /// <param name="isConfirmed">Debt confirmed status filter</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of debts</returns>
        Task<Response<DebtEntry>> GetDebtsAsync(
            int offset = 0,
            int limit = 10,
            string? lenderId = null,
            string? debtorId = null,
            bool? isSent = null,
            bool? isConfirmed = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get debts for specific user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="offset">Number of records to skip</param>
        /// <param name="limit">Maximum number of records to return</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of user debts</returns>
        Task<Response<DebtEntry>> GetDebtsByUserAsync(
            string userId,
            int offset = 0,
            int limit = 10,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Add new debt
        /// </summary>
        /// <param name="debt">Debt data to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created debt entry</returns>
        Task<DebtEntry> AddDebtAsync(DebtEntry debt, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete debt by identifier
        /// </summary>
        /// <param name="debtId">Debt identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeleteDebtAsync(Guid debtId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update debt information
        /// </summary>
        /// <param name="debt">Updated debt data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated debt entry</returns>
        Task<DebtEntry> UpdateDebtAsync(DebtEntry debt, CancellationToken cancellationToken = default);
    }
}