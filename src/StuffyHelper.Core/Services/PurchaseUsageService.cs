using AutoMapper;
using EnsureThat;
using Reg00.Infrastructure.Errors;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Contracts;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Models;
using StuffyHelper.Core.Services.Interfaces;
using StuffyHelper.Data.Repository.Interfaces;

namespace StuffyHelper.Core.Services
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

            return _mapper.Map<GetPurchaseUsageEntry>((entry, _mapper.Map<ParticipantShortEntry>((entry.Participant ,_mapper.Map<UserShortEntry>(claims)))));
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
                Data = resp.Data.Select(x => _mapper.Map<PurchaseUsageShortEntry>(x)),
                TotalPages = resp.TotalPages,
                Total = resp.Total
            };
        }

        public async Task<PurchaseUsageShortEntry> AddPurchaseUsageAsync(UpsertPurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchaseUsage, nameof(purchaseUsage));

            var entry = _mapper.Map<PurchaseUsageEntry>(purchaseUsage);
            var result = await _purchaseUsageStore.AddPurchaseUsageAsync(entry, cancellationToken);

            return _mapper.Map<PurchaseUsageShortEntry>(result);
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

            return _mapper.Map<PurchaseUsageShortEntry>(result);
        }
    }
}
