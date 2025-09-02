using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Data.Repository.Interfaces
{
    /// <summary>
    /// Interface for work with purchase usage repository
    /// </summary>
    public interface IPurchaseUsageRepository
    {
        /// <summary>
        /// Get purchase usage by identifier
        /// </summary>
        /// <param name="purchaseUsageId">Purchase usage identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Purchase usage entry</returns>
        Task<PurchaseUsageEntry> GetPurchaseUsageAsync(Guid purchaseUsageId, CancellationToken cancellationToken);

        /// <summary>
        /// Get filtered list of purchase usages
        /// </summary>
        /// <param name="offset">Number of records to skip</param>
        /// <param name="limit">Maximum number of records to return</param>
        /// <param name="eventId">Event identifier filter</param>
        /// <param name="participantId">Participant identifier filter</param>
        /// <param name="purchaseId">Purchase identifier filter</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of purchase usages</returns>
        Task<Response<PurchaseUsageEntry>> GetPurchaseUsagesAsync(
            int offset = 0,
            int limit = 10,
            Guid? eventId = null,
            Guid? participantId = null,
            Guid? purchaseId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Add new purchase usage
        /// </summary>
        /// <param name="purchaseUsage">Purchase usage data to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created purchase usage entry</returns>
        Task<PurchaseUsageEntry> AddPurchaseUsageAsync(PurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete purchase usage by identifier
        /// </summary>
        /// <param name="purchaseUsageId">Purchase usage identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeletePurchaseUsageAsync(Guid purchaseUsageId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update purchase usage information
        /// </summary>
        /// <param name="purchaseUsage">Updated purchase usage data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated purchase usage entry</returns>
        Task<PurchaseUsageEntry> UpdatePurchaseUsageAsync(PurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default);
    }
}