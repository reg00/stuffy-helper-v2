using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services.Interfaces
{
    public interface IPurchaseTagService
    {
        Task<GetPurchaseTagEntry> GetPurchaseTagAsync(string token, Guid purchaseTagId, CancellationToken cancellationToken);

        Task<Response<PurchaseTagShortEntry>> GetPurchaseTagsAsync(
            string token,
            int offset = 0,
            int limit = 10,
            string? name = null,
            Guid? purchaseId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default);

        Task<PurchaseTagShortEntry> AddPurchaseTagAsync(string token, UpsertPurchaseTagEntry purchaseTag, CancellationToken cancellationToken = default);

        Task DeletePurchaseTagAsync(string token, Guid purchaseTagId, CancellationToken cancellationToken = default);

        Task<PurchaseTagShortEntry> UpdatePurchaseTagAsync(string token, Guid purchaseTagId, UpsertPurchaseTagEntry purchaseTag, CancellationToken cancellationToken = default);
    }
}
