using StuffyHelper.ApiGateway.Core.Services.Interfaces;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Clients.Interface;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services
{
    public class PurchaseUsageService : IPurchaseUsageService
    {
        private readonly IPurchaseUsageClient _purchaseUsageClient;
        
        public PurchaseUsageService(IPurchaseUsageClient purchaseUsageClient)
        {
            _purchaseUsageClient = purchaseUsageClient;
        }

        public async Task<GetPurchaseUsageEntry> GetPurchaseUsageAsync(string token, Guid eventId, Guid purchaseUsageId, CancellationToken cancellationToken)
        {
            return await _purchaseUsageClient.GetPurchaseUsageAsync(token, eventId, purchaseUsageId, cancellationToken);
        }

        public async Task<Response<PurchaseUsageShortEntry>> GetPurchaseUsagesAsync(
            string token, 
            Guid eventId,
            int offset = 0,
            int limit = 10,
            Guid? participantId = null,
            Guid? purchaseId = null,
            CancellationToken cancellationToken = default)
        {
            return await _purchaseUsageClient.GetPurchaseUsagesAsync(token, eventId, offset, limit, participantId, purchaseId, cancellationToken);
        }

        public async Task<PurchaseUsageShortEntry> AddPurchaseUsageAsync(string token, Guid eventId, UpsertPurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default)
        {
            return await _purchaseUsageClient.CreatePurchaseUsageAsync(token, eventId, purchaseUsage, cancellationToken);
        }

        public async Task DeletePurchaseUsageAsync(string token, Guid eventId, Guid purchaseUsageId, CancellationToken cancellationToken = default)
        {
            await _purchaseUsageClient.DeletePurchaseUsageAsync(token, eventId, purchaseUsageId, cancellationToken);
        }

        public async Task<PurchaseUsageShortEntry> UpdatePurchaseUsageAsync(string token, Guid eventId, Guid purchaseUsageId, UpsertPurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default)
        {
            return await _purchaseUsageClient.UpdatePurchaseUsageAsync(token, eventId, purchaseUsageId, purchaseUsage, cancellationToken);
        }
    }
}
