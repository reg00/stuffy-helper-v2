using EnsureThat;
using Reg00.Infrastructure.Errors;
using StuffyHelper.Authorization.Core.Features;
using StuffyHelper.Authorization.Core.Models.User;
using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.PurchaseUsage
{
    public class PurchaseUsageService : IPurchaseUsageService
    {
        private readonly IPurchaseUsageStore _purchaseUsageStore;
        private readonly IAuthorizationService _authorizationService;
        public PurchaseUsageService(IPurchaseUsageStore purchaseUsageStore, IAuthorizationService authorizationService)
        {
            _purchaseUsageStore = purchaseUsageStore;
            _authorizationService = authorizationService;
        }

        public async Task<GetPurchaseUsageEntry> GetPurchaseUsageAsync(Guid purchaseUsageId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(purchaseUsageId, nameof(purchaseUsageId));

            var entry = await _purchaseUsageStore.GetPurchaseUsageAsync(purchaseUsageId, cancellationToken);
            var user = await _authorizationService.GetUser(userId: entry.Participant.UserId);

            return new GetPurchaseUsageEntry(entry, new UserShortEntry(user));
        }

        public async Task<Response<GetPurchaseUsageEntry>> GetPurchaseUsagesAsync(
            int offset = 0,
            int limit = 10,
            Guid? participantId = null,
            Guid? purchaseId = null,
            CancellationToken cancellationToken = default)
        {
            var resp = await _purchaseUsageStore.GetPurchaseUsagesAsync(offset, limit, participantId, purchaseId, cancellationToken);

            var purchaseUsages = new List<GetPurchaseUsageEntry>();
            foreach (var pu in resp.Data)
            {
                var user = await _authorizationService.GetUser(userId: pu.Participant.UserId);
                purchaseUsages.Add(new GetPurchaseUsageEntry(pu, new UserShortEntry(user)));
            }

            return new Response<GetPurchaseUsageEntry>()
            {
                Data = purchaseUsages,
                TotalPages = resp.TotalPages,
                Total = resp.Total
            };
        }

        public async Task<PurchaseUsageShortEntry> AddPurchaseUsageAsync(UpsertPurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchaseUsage, nameof(purchaseUsage));

            var entry = purchaseUsage.ToCommonEntry();
            var result = await _purchaseUsageStore.AddPurchaseUsageAsync(entry, cancellationToken);

            return new PurchaseUsageShortEntry(result);
        }

        public async Task DeletePurchaseUsageAsync(Guid purchaseUsageId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(purchaseUsageId, nameof(purchaseUsageId));

            await _purchaseUsageStore.DeletePurchaseUsageAsync(purchaseUsageId, cancellationToken);
        }

        public async Task<PurchaseUsageShortEntry> UpdatePurchaseUsageAsync(Guid purchaseUsageId, UpsertPurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchaseUsage, nameof(purchaseUsage));
            EnsureArg.IsNotDefault(purchaseUsageId, nameof(purchaseUsageId));

            var existingPurchaseUsage = await _purchaseUsageStore.GetPurchaseUsageAsync(purchaseUsageId, cancellationToken);

            if (existingPurchaseUsage is null)
            {
                throw new EntityNotFoundException($"PurchaseUsage Id '{purchaseUsageId}' not found");
            }

            existingPurchaseUsage.PatchFrom(purchaseUsage);
            var result = await _purchaseUsageStore.UpdatePurchaseUsageAsync(existingPurchaseUsage, cancellationToken);

            return new PurchaseUsageShortEntry(result);
        }
    }
}
