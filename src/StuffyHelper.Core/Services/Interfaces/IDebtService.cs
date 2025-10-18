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
        /// <param name="eventId">Event identifier</param>
        /// <param name="debtId">Debt identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Detailed debt information</returns>
        Task<GetDebtEntry> GetDebtAsync(Guid eventId, Guid debtId, CancellationToken cancellationToken);
        
        /// <summary>
        /// Get debts for specific user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="eventId">Event identifier</param>
        /// <param name="offset">Number of records to skip</param>
        /// <param name="limit">Maximum number of records to return</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of user debts</returns>
        Task<Response<GetDebtEntry>> GetDebtsByUserAsync(
            string userId,
            Guid eventId,
            int offset = 0,
            int limit = 10,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Send debt to debtor
        /// </summary>
        /// <param name="userId">User identifier (lender)</param>
        /// <param name="eventId">Event identifier</param>
        /// <param name="debtId">Debt identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated debt information</returns>
        Task<GetDebtEntry> SendDebtAsync(string userId, Guid eventId, Guid debtId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Confirm debt by debtor
        /// </summary>
        /// <param name="userId">User identifier (debtor)</param>
        /// <param name="eventId">Event identifier</param>
        /// <param name="debtId">Debt identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated debt information</returns>
        Task<GetDebtEntry> ConfirmDebtAsync(string userId, Guid eventId, Guid debtId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Calculate and create debts for event participants
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="userId">User identifier initiating checkout</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task CheckoutEvent(Guid eventId, string? userId, CancellationToken cancellationToken = default);
    }
}