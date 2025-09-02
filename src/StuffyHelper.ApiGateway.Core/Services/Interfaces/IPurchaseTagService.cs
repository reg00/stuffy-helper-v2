using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services.Interfaces
{
    /// <summary>
    /// Interface for work with purchase tags in API Gateway
    /// </summary>
    public interface IPurchaseTagService
    {
        /// <summary>
        /// Get purchase tag by identifier
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="purchaseTagId">Purchase tag identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Detailed purchase tag information</returns>
        Task<GetPurchaseTagEntry> GetPurchaseTagAsync(string token, Guid purchaseTagId, CancellationToken cancellationToken);

        /// <summary>
        /// Get filtered list of purchase tags
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="offset">Number of records to skip</param>
        /// <param name="limit">Maximum number of records to return</param>
        /// <param name="name">Purchase tag name filter</param>
        /// <param name="purchaseId">Purchase identifier filter</param>
        /// <param name="isActive">Purchase tag activity status filter</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of purchase tags</returns>
        Task<Response<PurchaseTagShortEntry>> GetPurchaseTagsAsync(
            string token,
            int offset = 0,
            int limit = 10,
            string? name = null,
            Guid? purchaseId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Add new purchase tag
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="purchaseTag">Purchase tag data to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created purchase tag information</returns>
        Task<PurchaseTagShortEntry> AddPurchaseTagAsync(string token, UpsertPurchaseTagEntry purchaseTag, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete purchase tag by identifier
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="purchaseTagId">Purchase tag identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeletePurchaseTagAsync(string token, Guid purchaseTagId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update purchase tag information
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="purchaseTagId">Purchase tag identifier</param>
        /// <param name="purchaseTag">Updated purchase tag data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated purchase tag information</returns>
        Task<PurchaseTagShortEntry> UpdatePurchaseTagAsync(string token, Guid purchaseTagId, UpsertPurchaseTagEntry purchaseTag, CancellationToken cancellationToken = default);
    }
}