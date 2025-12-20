using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Data.Repository.Interfaces
{
    /// <summary>
    /// Interface for work with checkout repository
    /// </summary>
    public interface ICheckoutRepository
    {
        /// <summary>
        /// Get checkout by identifier
        /// </summary>
        /// <param name="checkoutId">Checkout identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Checkout entry</returns>
        Task<CheckoutEntry> GetCheckoutAsync(Guid checkoutId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Get checkouts by event identifier
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of checkouts for the event</returns>
        Task<Response<CheckoutEntry>> GetCheckoutsByEventIdAsync(Guid eventId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Add new checkout
        /// </summary>
        /// <param name="checkout">Checkout data to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created checkout entry</returns>
        Task<CheckoutEntry> AddCheckoutAsync(CheckoutEntry checkout, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Delete checkout by identifier
        /// </summary>
        /// <param name="checkoutId">Checkout identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeleteCheckoutAsync(Guid checkoutId, CancellationToken cancellationToken = default);
    }
}