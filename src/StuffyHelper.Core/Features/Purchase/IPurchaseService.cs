﻿using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.Purchase
{
    public interface IPurchaseService
    {
        Task<GetPurchaseEntry> GetPurchaseAsync(Guid purchaseId, CancellationToken cancellationToken);

        Task<Response<PurchaseShortEntry>> GetPurchasesAsync(
            int offset = 0,
            int limit = 10,
            string name = null,
            int? countMin = null,
            int? countMax = null,
            double? costMin = null,
            double? costMax = null,
            double? weightMin = null,
            double? weightMax = null,
            Guid? shoppingId = null,
            IEnumerable<string> purchaseTags = null,
            Guid? unitTypeId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default);

        Task<PurchaseShortEntry> AddPurchaseAsync(UpsertPurchaseEntry purchase, CancellationToken cancellationToken = default);

        Task DeletePurchaseAsync(Guid purchaseId, CancellationToken cancellationToken = default);

        Task<PurchaseShortEntry> UpdatePurchaseAsync(Guid purchaseId, UpsertPurchaseEntry purchase, CancellationToken cancellationToken = default);
    }
}
