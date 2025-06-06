using AutoMapper;
using EnsureThat;
using Reg00.Infrastructure.Errors;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Contracts;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Data.Repository.Interfaces;

namespace StuffyHelper.Core.Features.PurchaseUsage
{
    public class PurchaseUsageService : IPurchaseUsageService
    {
        private readonly IPurchaseUsageRepository _purchaseUsageStore;
        private readonly IMapper _mapper;
        
        public PurchaseUsageService(IPurchaseUsageRepository purchaseUsageStore, IMapper mapper)
        {
            _purchaseUsageStore = purchaseUsageStore;
            _mapper = mapper;
        }

        public async Task<GetPurchaseUsageEntry> GetPurchaseUsageAsync(StuffyClaims claims, Guid purchaseUsageId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(purchaseUsageId, nameof(purchaseUsageId));

            var entry = await _purchaseUsageStore.GetPurchaseUsageAsync(purchaseUsageId, cancellationToken);

            return new GetPurchaseUsageEntry(entry, _mapper.Map<UserShortEntry>(claims));
        }

        public async Task<Response<PurchaseUsageShortEntry>> GetPurchaseUsagesAsync(
            int offset = 0,
            int limit = 10,
            Guid? eventId = null,
            Guid? participantId = null,
            Guid? purchaseId = null,
            CancellationToken cancellationToken = default)
        {
            var resp = await _purchaseUsageStore.GetPurchaseUsagesAsync(offset, limit, eventId, participantId, purchaseId, cancellationToken);

            return new Response<PurchaseUsageShortEntry>()
            {
                Data = resp.Data.Select(x => new PurchaseUsageShortEntry(x)),
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
