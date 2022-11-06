using EnsureThat;
using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.Shopping
{
    public class ShoppingService : IShoppingService
    {
        private readonly IShoppingStore _shoppingStore;

        public ShoppingService(IShoppingStore shoppingStore)
        {
            _shoppingStore = shoppingStore;
        }

        public async Task<GetShoppingEntry> GetShoppingAsync(Guid shoppingId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(shoppingId, nameof(shoppingId));

            var entry = await _shoppingStore.GetShoppingAsync(shoppingId, cancellationToken);

            return new GetShoppingEntry(entry);
        }

        public async Task<Response<ShoppingShortEntry>> GetShoppingsAsync(
            int offset = 0,
            int limit = 10,
            DateTime? shoppingDateStart = null,
            DateTime? shoppingDateEnd = null,
            Guid? participantId = null,
            Guid? eventId = null,
            string description = null,
            CancellationToken cancellationToken = default)
        {
            var resp = await _shoppingStore.GetShoppingsAsync(offset, limit, shoppingDateStart, shoppingDateEnd,
                                                               participantId, eventId, description, cancellationToken);

            return new Response<ShoppingShortEntry>()
            {
                Data = resp.Data.Select(x => new ShoppingShortEntry(x)),
                TotalPages = resp.TotalPages,
                Total = resp.Total
            };
        }

        public async Task<ShoppingShortEntry> AddShoppingAsync(AddShoppingEntry shopping, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(shopping, nameof(shopping));

            var entry = shopping.ToCommonEntry();
            var result = await _shoppingStore.AddShoppingAsync(entry, cancellationToken);

            return new ShoppingShortEntry(result);
        }

        public async Task DeleteShoppingAsync(Guid shoppingId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(shoppingId, nameof(shoppingId));

            await _shoppingStore.DeleteShoppingAsync(shoppingId, cancellationToken);
        }

        public async Task<ShoppingShortEntry> UpdateShoppingAsync(Guid shoppingId, UpdateShoppingEntry shopping, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(shopping, nameof(shopping));
            EnsureArg.IsNotDefault(shoppingId, nameof(shoppingId));

            var existingShopping = await _shoppingStore.GetShoppingAsync(shoppingId, cancellationToken);

            if (existingShopping is null)
            {
                throw new ResourceNotFoundException($"Shopping Id '{shoppingId}' not found");
            }

            existingShopping.PatchFrom(shopping);
            var result = await _shoppingStore.UpdateShoppingAsync(existingShopping, cancellationToken);

            return new ShoppingShortEntry(result);
        }
    }
}
