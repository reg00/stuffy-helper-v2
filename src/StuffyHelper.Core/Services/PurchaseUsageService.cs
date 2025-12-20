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
    /// <inheritdoc />
    public class PurchaseUsageService : IPurchaseUsageService
    {
        private readonly IPurchaseUsageRepository _purchaseUsageRepository;
        private readonly IMapper _mapper;
        
        /// <summary>
        /// Ctor.
        /// </summary>
        public PurchaseUsageService(IPurchaseUsageRepository purchaseUsageRepository, IMapper mapper)
        {
            _purchaseUsageRepository = purchaseUsageRepository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<GetPurchaseUsageEntry> GetPurchaseUsageAsync(StuffyClaims claims, Guid eventId, Guid purchaseUsageId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(purchaseUsageId, nameof(purchaseUsageId));

            var entry = await _purchaseUsageRepository.GetPurchaseUsageAsync(eventId, purchaseUsageId, cancellationToken);

            return _mapper.Map<GetPurchaseUsageEntry>((entry, _mapper.Map<ParticipantShortEntry>((entry.Participant ,_mapper.Map<UserShortEntry>(claims)))));
        }

        /// <inheritdoc />
        public async Task<Response<PurchaseUsageShortEntry>> GetPurchaseUsagesAsync(
            Guid eventId,
            int offset = 0,
            int limit = 10,
            Guid? participantId = null,
            Guid? purchaseId = null,
            CancellationToken cancellationToken = default)
        {
            var resp = await _purchaseUsageRepository.GetPurchaseUsagesAsync(eventId, offset, limit, participantId, purchaseId, cancellationToken);

            return new Response<PurchaseUsageShortEntry>()
            {
                Data = resp.Data.Select(x => _mapper.Map<PurchaseUsageShortEntry>(x)),
                TotalPages = resp.TotalPages,
                Total = resp.Total
            };
        }

        /// <inheritdoc />
        public async Task<PurchaseUsageShortEntry> AddPurchaseUsageAsync(Guid eventId, UpsertPurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchaseUsage, nameof(purchaseUsage));

            var entry = _mapper.Map<PurchaseUsageEntry>(purchaseUsage);
            var result = await _purchaseUsageRepository.AddPurchaseUsageAsync(entry, cancellationToken);

            return _mapper.Map<PurchaseUsageShortEntry>(result);
        }

        /// <inheritdoc />
        public async Task DeletePurchaseUsageAsync(Guid eventId, Guid purchaseUsageId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(purchaseUsageId, nameof(purchaseUsageId));

            await _purchaseUsageRepository.DeletePurchaseUsageAsync(eventId, purchaseUsageId, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<PurchaseUsageShortEntry> UpdatePurchaseUsageAsync(Guid eventId, Guid purchaseUsageId, UpsertPurchaseUsageEntry purchaseUsage, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchaseUsage, nameof(purchaseUsage));
            EnsureArg.IsNotDefault(purchaseUsageId, nameof(purchaseUsageId));

            var existingPurchaseUsage = await _purchaseUsageRepository.GetPurchaseUsageAsync(eventId, purchaseUsageId, cancellationToken);

            if (existingPurchaseUsage is null)
            {
                throw new EntityNotFoundException($"PurchaseUsage Id '{purchaseUsageId}' not found");
            }

            existingPurchaseUsage.PatchFrom(purchaseUsage);
            var result = await _purchaseUsageRepository.UpdatePurchaseUsageAsync(existingPurchaseUsage, cancellationToken);

            return _mapper.Map<PurchaseUsageShortEntry>(result);
        }
    }
}
