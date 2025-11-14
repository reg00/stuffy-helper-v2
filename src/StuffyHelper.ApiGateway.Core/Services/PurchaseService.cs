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

        public async Task<GetPurchaseEntry> GetPurchaseAsync(string token, Guid eventId, Guid purchaseId, CancellationToken cancellationToken)
        {
            return await _purchaseClient.GetPurchaseAsync(token, eventId, purchaseId, cancellationToken);
        }

        public async Task<Response<GetPurchaseEntry>> GetPurchasesAsync(
            string token, 
            Guid eventId,
            int offset = 0,
            int limit = 10,
            string? name = null,
            long? costMin = null,
            long? costMax = null,
            bool? isComplete = null,
            Guid[]? purchaseIds = null,
            CancellationToken cancellationToken = default)
        {
            return await _purchaseClient.GetPurchasesAsync(token, eventId, offset, limit, name, costMin, costMax,
                                                              isComplete, purchaseIds, cancellationToken);
        }

        public async Task<PurchaseShortEntry> AddPurchaseAsync(string token, Guid eventId, AddPurchaseEntry purchase, CancellationToken cancellationToken = default)
        {
            return await _purchaseClient.CreatePurchaseAsync(token, eventId, purchase, cancellationToken);
        }

        public async Task DeletePurchaseAsync(string token, Guid eventId, Guid purchaseId, CancellationToken cancellationToken = default)
        {
            await _purchaseClient.DeletePurchaseAsync(token, eventId, purchaseId, cancellationToken);
        }

        public async Task<PurchaseShortEntry> UpdatePurchaseAsync(string token, Guid eventId, Guid purchaseId, UpdatePurchaseEntry purchase, CancellationToken cancellationToken = default)
        {
            return await _purchaseClient.UpdatePurchaseAsync(token, eventId, purchaseId, purchase, cancellationToken);
        }
    }
}
