using StuffyHelper.ApiGateway.Core.Services.Interfaces;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Clients.Interface;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseClient _purchaseClient;

        public PurchaseService(IPurchaseClient purchaseClient)
        {
            _purchaseClient = purchaseClient;
        }

        public async Task<GetPurchaseEntry> GetPurchaseAsync(string token, Guid purchaseId, CancellationToken cancellationToken)
        {
            return await _purchaseClient.GetPurchaseAsync(token, purchaseId, cancellationToken);
        }

        public async Task<Response<GetPurchaseEntry>> GetPurchasesAsync(
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
            CancellationToken cancellationToken = default)
        {
            return await _purchaseClient.GetPurchasesAsync(token, offset, limit, name, costMin, costMax,
                                                              eventId, purchaseTags, unitTypeId, isComplete, cancellationToken);
        }

        public async Task<PurchaseShortEntry> AddPurchaseAsync(string token, AddPurchaseEntry purchase, CancellationToken cancellationToken = default)
        {
            return await _purchaseClient.CreatePurchaseAsync(token, purchase, cancellationToken);
        }

        public async Task DeletePurchaseAsync(string token, Guid purchaseId, CancellationToken cancellationToken = default)
        {
            await _purchaseClient.DeletePurchaseAsync(token, purchaseId, cancellationToken);
        }

        public async Task<PurchaseShortEntry> UpdatePurchaseAsync(string token, Guid purchaseId, UpdatePurchaseEntry purchase, CancellationToken cancellationToken = default)
        {
            return await _purchaseClient.UpdatePurchaseAsync(token, purchaseId, purchase, cancellationToken);
        }
    }
}
