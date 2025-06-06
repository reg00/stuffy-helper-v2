using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Data.Repository.Interfaces
{
    public interface ICheckoutRepository
    {
        Task<CheckoutEntry> GetCheckoutAsync(Guid checkoutId, CancellationToken cancellationToken = default);
        
        Task<Response<CheckoutEntry>> GetCheckoutsByEventIdAsync(Guid eventId, CancellationToken cancellationToken = default);
        
        Task<CheckoutEntry> AddCheckoutAsync(CheckoutEntry checkout, CancellationToken cancellationToken = default);
        
        Task DeleteCheckoutAsync(Guid checkoutId, CancellationToken cancellationToken = default);
    }
}
