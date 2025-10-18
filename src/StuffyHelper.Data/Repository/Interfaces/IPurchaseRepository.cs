using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Data.Repository.Interfaces
{
    /// <summary>
    /// Interface for work with purchase repository
    /// </summary>
    public interface IPurchaseRepository
    {
        /// <summary>
        /// Get purchase by identifier
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="purchaseId">Purchase identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Purchase entry</returns>
        Task<PurchaseEntry> GetPurchaseAsync(Guid eventId, Guid purchaseId, CancellationToken cancellationToken);

        /// <summary>
        /// Get filtered list of purchases
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="offset">Number of records to skip</param>
        /// <param name="limit">Maximum number of records to return</param>
        /// <param name="name">Purchase name filter</param>
        /// <param name="costMin">Minimum cost filter</param>
        /// <param name="costMax">Maximum cost filter</param>
        /// <param name="purchaseTags">Purchase tags filter</param>
        /// <param name="unitTypeId">Unit type identifier filter</param>
        /// <param name="isComplete">Purchase completion status filter</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of purchases</returns>
        Task<Response<PurchaseEntry>> GetPurchasesAsync(
            Guid eventId,
            int offset = 0,
            int limit = 10,
            string? name = null,
            double? costMin = null,
            double? costMax = null,
            IEnumerable<string>? purchaseTags = null,
            Guid? unitTypeId = null,
            bool? isComplete = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Add new purchase
        /// </summary>
        /// <param name="purchase">Purchase data to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created purchase entry</returns>
        Task<PurchaseEntry> AddPurchaseAsync(PurchaseEntry purchase, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete purchase by identifier
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="purchaseId">Purchase identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeletePurchaseAsync(Guid eventId, Guid purchaseId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update purchase information
        /// </summary>
        /// <param name="purchase">Updated purchase data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated purchase entry</returns>
        Task<PurchaseEntry> UpdatePurchaseAsync(PurchaseEntry purchase, CancellationToken cancellationToken = default);

        /// <summary>
        /// Mark purchase as completed
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="purchaseId">Purchase identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task CompletePurchaseAsync(Guid eventId, Guid purchaseId, CancellationToken cancellationToken = default);
    }
}