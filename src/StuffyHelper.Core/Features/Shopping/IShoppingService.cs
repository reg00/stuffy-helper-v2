using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.Shopping
{
    public interface IShoppingService
    {
        Task<GetShoppingEntry> GetShoppingAsync(Guid shoppingId, CancellationToken cancellationToken);

        Task<Response<GetShoppingEntry>> GetShoppingsAsync(
            int offset = 0,
            int limit = 10,
            DateTime? shoppingDateStart = null,
            DateTime? shoppingDateEnd = null,
            Guid? participantId = null,
            Guid? eventId = null,
            string description = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default);

        Task<GetShoppingEntry> AddShoppingAsync(UpsertShoppingEntry shopping, CancellationToken cancellationToken = default);

        Task DeleteShoppingAsync(Guid shoppingId, CancellationToken cancellationToken = default);

        Task<GetShoppingEntry> UpdateShoppingAsync(Guid shoppingId, UpsertShoppingEntry shopping, CancellationToken cancellationToken = default);
    }
}
