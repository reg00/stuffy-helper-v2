using StuffyHelper.ApiGateway.Core.Services.Interfaces;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Clients.Interface;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services
{
    public class PurchaseTagService : IPurchaseTagService
    {
        private readonly IPurchaseTagClient _purchaseTagClient;

        public PurchaseTagService(IPurchaseTagClient purchaseTagClient)
        {
            _purchaseTagClient = purchaseTagClient;
        }

        public async Task<GetPurchaseTagEntry> GetPurchaseTagAsync(string token, Guid purchaseTagId, CancellationToken cancellationToken)
        {
            return await _purchaseTagClient.GetPurchaseTagAsync(token, purchaseTagId, cancellationToken);
        }

        public async Task<Response<PurchaseTagShortEntry>> GetPurchaseTagsAsync(
            string token, 
            int offset = 0,
            int limit = 10,
            string? name = null,
            Guid? purchaseId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default)
        {
            return await _purchaseTagClient.GetPurchaseTagsAsync(token, offset, limit, name, purchaseId, isActive, cancellationToken);
        }

        public async Task<PurchaseTagShortEntry> AddPurchaseTagAsync(string token, UpsertPurchaseTagEntry purchaseTag, CancellationToken cancellationToken = default)
        {
            return await _purchaseTagClient.CreatePurchaseTagAsync(token, purchaseTag, cancellationToken);
        }

        public async Task DeletePurchaseTagAsync(string token, Guid purchaseTagId, CancellationToken cancellationToken = default)
        {
            await _purchaseTagClient.DeletePurchaseTagAsync(token, purchaseTagId, cancellationToken);
        }

        public async Task<PurchaseTagShortEntry> UpdatePurchaseTagAsync(string token, Guid purchaseTagId, UpsertPurchaseTagEntry purchaseTag, CancellationToken cancellationToken = default)
        {
            return await _purchaseTagClient.UpdatePurchaseTagAsync(token, purchaseTagId, purchaseTag, cancellationToken);
        }
    }
}
