using StuffyHelper.Contracts.Entities;
using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.Checkout
{
    public interface ICheckoutStore
    {
        Task<CheckoutEntry> GetCheckoutAsync(Guid checkoutId, CancellationToken cancellationToken = default);
        
        Task<Response<CheckoutEntry>> GetCheckoutsByEventIdAsync(Guid eventId, CancellationToken cancellationToken = default);
        
        Task<CheckoutEntry> AddCheckoutAsync(CheckoutEntry checkout, CancellationToken cancellationToken = default);
        
        Task DeleteCheckoutAsync(Guid checkoutId, CancellationToken cancellationToken = default);
    }
}
