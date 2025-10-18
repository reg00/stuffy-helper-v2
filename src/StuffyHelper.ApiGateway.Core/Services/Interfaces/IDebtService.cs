using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services.Interfaces
{
    /// <summary>
    /// Interface for work with debts in API Gateway
    /// </summary>
    public interface IDebtService
    {
        /// <summary>
        /// Get debt by identifier
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="eventId">Event id</param>
        /// <param name="debtId">Debt identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Detailed debt information</returns>
        Task<GetDebtEntry> GetDebtAsync(string token, Guid eventId, Guid debtId, CancellationToken cancellationToken);

        /// <summary>
        /// Get debts for specific user
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="eventId">Event id</param>
        /// <param name="offset">Number of records to skip</param>
        /// <param name="limit">Maximum number of records to return</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of user debts</returns>
        Task<Response<GetDebtEntry>> GetDebtsByUserAsync(
            string token,
            Guid eventId,
            int offset = 0,
            int limit = 10,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Send debt to debtor
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="eventId">Event id</param>
        /// <param name="debtId">Debt identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task SendDebtAsync(string token, Guid eventId, Guid debtId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Confirm debt by debtor
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="eventId">Event id</param>
        /// <param name="debtId">Debt identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task ConfirmDebtAsync(string token, Guid eventId, Guid debtId, CancellationToken cancellationToken = default);
    }
}