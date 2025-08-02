using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services.Interfaces
{
    public interface IPurchaseUsageService
    {

        Task<GetPurchaseUsageEntry> GetPurchaseUsageAsync(string token, Guid purchaseUsageId, CancellationToken cancellationToken);

        Task<Response<PurchaseUsageShortEntry>> GetPurchaseUsagesAsync(
            string token,
            int offset = 0,
            int limit = 10,
            Guid? eventId = null,
            Guid? participantId = null,
            Guid? purchaseId = null,
            CancellationToken cancellationToken = default);

        Task<PurchaseUsageShortEntry> AddPurchaseUsageAsync(string token, UpsertPurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default);

        Task DeletePurchaseUsageAsync(string token, Guid purchaseUsageId, CancellationToken cancellationToken = default);

        Task<PurchaseUsageShortEntry> UpdatePurchaseUsageAsync(string token, Guid purchaseUsageId, UpsertPurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default);
    }
}
