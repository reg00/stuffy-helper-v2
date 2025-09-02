using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services.Interfaces
{
    /// <summary>
    /// Interface for work with purchases in API Gateway
    /// </summary>
    public interface IPurchaseService
    {
        /// <summary>
        /// Get purchase by identifier
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="purchaseId">Purchase identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Detailed purchase information</returns>
        Task<GetPurchaseEntry> GetPurchaseAsync(string token, Guid purchaseId, CancellationToken cancellationToken);

        /// <summary>
        /// Get filtered list of purchases
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="offset">Number of records to skip</param>
        /// <param name="limit">Maximum number of records to return</param>
        /// <param name="name">Purchase name filter</param>
        /// <param name="costMin">Minimum cost filter</param>
        /// <param name="costMax">Maximum cost filter</param>
        /// <param name="eventId">Event identifier filter</param>
        /// <param name="purchaseTags">Purchase tags filter</param>
        /// <param name="unitTypeId">Unit type identifier filter</param>
        /// <param name="isComplete">Purchase completion status filter</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of purchases</returns>
        Task<Response<GetPurchaseEntry>> GetPurchasesAsync(
            string token,
            int offset = 0,
            int limit = 10,
            string? name = null,
            double? costMin = null,
            double? costMax = null,
            Guid? eventId = null,
            string[]? purchaseTags = null,
            Guid? unitTypeId = null,
            bool? isComplete = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Add new purchase
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="purchase">Purchase data to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created purchase information</returns>
        Task<PurchaseShortEntry> AddPurchaseAsync(string token, AddPurchaseEntry purchase, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete purchase by identifier
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="purchaseId">Purchase identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeletePurchaseAsync(string token, Guid purchaseId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update purchase information
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="purchaseId">Purchase identifier</param>
        /// <param name="purchase">Updated purchase data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated purchase information</returns>
        Task<PurchaseShortEntry> UpdatePurchaseAsync(string token, Guid purchaseId,  UpdatePurchaseEntry purchase, CancellationToken cancellationToken = default);
    }
}