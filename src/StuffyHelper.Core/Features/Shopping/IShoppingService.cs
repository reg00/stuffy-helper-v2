using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.Shopping
{
    public interface IShoppingService
    {
        Task<GetShoppingEntry> GetShoppingAsync(Guid shoppingId, CancellationToken cancellationToken);

        Task<Response<ShoppingShortEntry>> GetShoppingsAsync(
            int offset = 0,
            int limit = 10,
            DateTime? shoppingDateStart = null,
            DateTime? shoppingDateEnd = null,
            Guid? participantId = null,
            Guid? eventId = null,
            string description = null,
            CancellationToken cancellationToken = default);

        Task<ShoppingShortEntry> AddShoppingAsync(AddShoppingEntry shopping, CancellationToken cancellationToken = default);

        Task DeleteShoppingAsync(Guid shoppingId, CancellationToken cancellationToken = default);

        Task<ShoppingShortEntry> UpdateShoppingAsync(Guid shoppingId, UpdateShoppingEntry shopping, CancellationToken cancellationToken = default);
    }
}
