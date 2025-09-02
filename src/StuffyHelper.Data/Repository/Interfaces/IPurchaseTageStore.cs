using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Data.Repository.Interfaces
{
    /// <summary>
    /// Interface for work with purchase tag repository
    /// </summary>
    public interface IPurchaseTagRepository
    {
        /// <summary>
        /// Get purchase tag by identifier
        /// </summary>
        /// <param name="purchaseTagId">Purchase tag identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Purchase tag entry</returns>
        Task<PurchaseTagEntry> GetPurchaseTagAsync(Guid purchaseTagId, CancellationToken cancellationToken);
        
        /// <summary>
        /// Get purchase tag by name
        /// </summary>
        /// <param name="tag">Purchase tag name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Purchase tag entry</returns>
        Task<PurchaseTagEntry> GetPurchaseTagAsync(string tag, CancellationToken cancellationToken);

        /// <summary>
        /// Get filtered list of purchase tags
        /// </summary>
        /// <param name="offset">Number of records to skip</param>
        /// <param name="limit">Maximum number of records to return</param>
        /// <param name="name">Purchase tag name filter</param>
        /// <param name="purchaseId">Purchase identifier filter</param>
        /// <param name="isActive">Purchase tag activity status filter</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of purchase tags</returns>
        Task<Response<PurchaseTagEntry>> GetPurchaseTagsAsync(
            int offset = 0,
            int limit = 10,
            string? name = null,
            Guid? purchaseId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Add new purchase tag
        /// </summary>
        /// <param name="purchaseTag">Purchase tag data to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created purchase tag entry</returns>
        Task<PurchaseTagEntry> AddPurchaseTagAsync(PurchaseTagEntry purchaseTag, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete purchase tag by identifier
        /// </summary>
        /// <param name="purchaseTagId">Purchase tag identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeletePurchaseTagAsync(Guid purchaseTagId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update purchase tag information
        /// </summary>
        /// <param name="purchaseTag">Updated purchase tag data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated purchase tag entry</returns>
        Task<PurchaseTagEntry> UpdatePurchaseTagAsync(PurchaseTagEntry purchaseTag, CancellationToken cancellationToken = default);
    }
}