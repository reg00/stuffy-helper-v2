using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services.Interfaces
{
    public interface IPurchaseService
    {
        Task<GetPurchaseEntry> GetPurchaseAsync(string token, Guid purchaseId, CancellationToken cancellationToken);

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

        Task<PurchaseShortEntry> AddPurchaseAsync(string token, AddPurchaseEntry purchase, CancellationToken cancellationToken = default);

        Task DeletePurchaseAsync(string token, Guid purchaseId, CancellationToken cancellationToken = default);

        Task<PurchaseShortEntry> UpdatePurchaseAsync(string token, Guid purchaseId,  UpdatePurchaseEntry purchase, CancellationToken cancellationToken = default);
    }
}
