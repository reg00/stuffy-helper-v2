using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.Shopping
{
    public interface IShoppingStore
    {
        Task<ShoppingEntry> GetShoppingAsync(Guid shoppingId, CancellationToken cancellationToken);

        Task<Response<ShoppingEntry>> GetShoppingsAsync(
            int offset = 0,
            int limit = 10,
            DateTime? shoppingDateStart = null,
            DateTime? shoppingDateEnd = null,
            Guid? participantId = null,
            Guid? eventId = null,
            string description = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default);

        Task<ShoppingEntry> AddShoppingAsync(ShoppingEntry shopping, CancellationToken cancellationToken = default);

        Task DeleteShoppingAsync(Guid shoppingId, CancellationToken cancellationToken = default);

        Task<ShoppingEntry> UpdateShoppingAsync(ShoppingEntry shopping, CancellationToken cancellationToken = default);
    }
}
