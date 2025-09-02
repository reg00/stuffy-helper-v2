using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Core.Services.Interfaces
{
    /// <summary>
    /// Interface for work with debts
    /// </summary>
    public interface IDebtService
    {
        /// <summary>
        /// Get debt by identifier
        /// </summary>
        /// <param name="debtId">Debt identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Detailed debt information</returns>
        Task<GetDebtEntry> GetDebtAsync(Guid debtId, CancellationToken cancellationToken);

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
        Task<Response<GetDebtEntry>> GetDebtsAsync(
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
        Task<Response<GetDebtEntry>> GetDebtsByUserAsync(
            string userId,
            int offset = 0,
            int limit = 10,
            CancellationToken cancellationToken = default);

        //Task<GetDebtEntry> AddDebtAsync(DebtEntry debt, CancellationToken cancellationToken = default);

        //Task DeleteDebtAsync(Guid debtId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send debt to debtor
        /// </summary>
        /// <param name="userId">User identifier (lender)</param>
        /// <param name="debtId">Debt identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated debt information</returns>
        Task<GetDebtEntry> SendDebtAsync(string userId, Guid debtId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Confirm debt by debtor
        /// </summary>
        /// <param name="userId">User identifier (debtor)</param>
        /// <param name="debtId">Debt identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated debt information</returns>
        Task<GetDebtEntry> ConfirmDebtAsync(string userId, Guid debtId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Calculate and create debts for event participants
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="userId">User identifier initiating checkout</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task CheckoutEvent(Guid eventId, string? userId, CancellationToken cancellationToken = default);
    }
}