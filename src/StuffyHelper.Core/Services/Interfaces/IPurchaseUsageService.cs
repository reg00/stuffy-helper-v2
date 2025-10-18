using StuffyHelper.Common.Contracts;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Core.Services.Interfaces
{
    /// <summary>
    /// Interface for work with purchase usages
    /// </summary>
    public interface IPurchaseUsageService
    {
        /// <summary>
        /// Get purchase usage by identifier
        /// </summary>
        /// <param name="claims">User claims for authorization</param>
        /// <param name="eventId">Event identifier</param>
        /// <param name="purchaseUsageId">Purchase usage identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Detailed purchase usage information</returns>
        Task<GetPurchaseUsageEntry> GetPurchaseUsageAsync(StuffyClaims claims, Guid eventId, Guid purchaseUsageId, CancellationToken cancellationToken);

        /// <summary>
        /// Get filtered list of purchase usages
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="offset">Number of records to skip</param>
        /// <param name="limit">Maximum number of records to return</param>
        /// <param name="participantId">Participant identifier filter</param>
        /// <param name="purchaseId">Purchase identifier filter</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of purchase usages</returns>
        Task<Response<PurchaseUsageShortEntry>> GetPurchaseUsagesAsync(
            Guid eventId,
            int offset = 0,
            int limit = 10,
            Guid? participantId = null,
            Guid? purchaseId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Add new purchase usage
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="purchaseUsage">Purchase usage data to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created purchase usage information</returns>
        Task<PurchaseUsageShortEntry> AddPurchaseUsageAsync(Guid eventId, UpsertPurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete purchase usage by identifier
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="purchaseUsageId">Purchase usage identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeletePurchaseUsageAsync(Guid eventId, Guid purchaseUsageId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update purchase usage information
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="purchaseUsageId">Purchase usage identifier</param>
        /// <param name="purchaseUsage">Updated purchase usage data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated purchase usage information</returns>
        Task<PurchaseUsageShortEntry> UpdatePurchaseUsageAsync(Guid eventId, Guid purchaseUsageId, UpsertPurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default);
    }
}