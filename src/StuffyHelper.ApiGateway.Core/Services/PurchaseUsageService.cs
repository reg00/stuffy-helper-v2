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

        public async Task<GetPurchaseUsageEntry> GetPurchaseUsageAsync(string token, Guid purchaseUsageId, CancellationToken cancellationToken)
        {
            return await _purchaseUsageClient.GetPurchaseUsageAsync(token, purchaseUsageId, cancellationToken);
        }

        public async Task<Response<PurchaseUsageShortEntry>> GetPurchaseUsagesAsync(
            string token, 
            int offset = 0,
            int limit = 10,
            Guid? eventId = null,
            Guid? participantId = null,
            Guid? purchaseId = null,
            CancellationToken cancellationToken = default)
        {
            return await _purchaseUsageClient.GetPurchaseUsagesAsync(token, offset, limit, eventId, participantId, purchaseId, cancellationToken);
        }

        public async Task<PurchaseUsageShortEntry> AddPurchaseUsageAsync(string token, UpsertPurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default)
        {
            return await _purchaseUsageClient.CreatePurchaseUsageAsync(token, purchaseUsage, cancellationToken);
        }

        public async Task DeletePurchaseUsageAsync(string token, Guid purchaseUsageId, CancellationToken cancellationToken = default)
        {
            await _purchaseUsageClient.DeletePurchaseUsageAsync(token, purchaseUsageId, cancellationToken);
        }

        public async Task<PurchaseUsageShortEntry> UpdatePurchaseUsageAsync(string token, Guid purchaseUsageId, UpsertPurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default)
        {
            return await _purchaseUsageClient.UpdatePurchaseUsageAsync(token, purchaseUsageId, purchaseUsage, cancellationToken);
        }
    }
}
