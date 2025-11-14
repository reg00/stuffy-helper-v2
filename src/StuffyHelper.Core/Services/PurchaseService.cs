using AutoMapper;
using EnsureThat;
using StuffyHelper.Common.Exceptions;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Models;
using StuffyHelper.Core.Services.Interfaces;
using StuffyHelper.Data.Repository.Interfaces;
using EntityNotFoundException = Reg00.Infrastructure.Errors.EntityNotFoundException;

namespace StuffyHelper.Core.Services
{
    /// <inheritdoc />
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Ctor.
        /// </summary>
        public PurchaseService(
            IPurchaseRepository purchaseRepository,
            IMapper mapper)
        {
            _purchaseRepository = purchaseRepository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<GetPurchaseEntry> GetPurchaseAsync(Guid eventId, Guid purchaseId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(purchaseId, nameof(purchaseId));
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            var entry = await _purchaseRepository.GetPurchaseAsync(eventId, purchaseId, cancellationToken);

            return _mapper.Map<GetPurchaseEntry>(entry);
        }

        /// <inheritdoc />
        public async Task<Response<GetPurchaseEntry>> GetPurchasesAsync(
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
            var resp = await _purchaseRepository.GetPurchasesAsync(eventId, offset, limit, name, costMin, costMax,
                                                              isComplete, purchaseIds, cancellationToken);

            return new Response<GetPurchaseEntry>()
            {
                Data = resp.Data.Select(x => _mapper.Map<GetPurchaseEntry>(x)),
                TotalPages = resp.TotalPages,
                Total = resp.Total
            };
        }

        /// <inheritdoc />
        public async Task<PurchaseShortEntry> AddPurchaseAsync(Guid eventId, AddPurchaseEntry purchase, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchase, nameof(purchase));

            var entry = _mapper.Map<PurchaseEntry>(purchase);
            entry.EventId = eventId;
            var result = await _purchaseRepository.AddPurchaseAsync(entry, cancellationToken);

            return _mapper.Map<PurchaseShortEntry>(result);
        }

        /// <inheritdoc />
        public async Task DeletePurchaseAsync(Guid eventId, Guid purchaseId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(purchaseId, nameof(purchaseId));
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            await ValidatePurchase(eventId, purchaseId, cancellationToken);

            await _purchaseRepository.DeletePurchaseAsync(eventId, purchaseId, cancellationToken);
        }

        /// <inheritdoc />
        public async Task CompletePurchaseAsync(Guid eventId, Guid purchaseId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(purchaseId, nameof(purchaseId));
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            await ValidatePurchase(eventId, purchaseId, cancellationToken);

            await _purchaseRepository.CompletePurchaseAsync(eventId, purchaseId, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<PurchaseShortEntry> UpdatePurchaseAsync(Guid eventId, Guid purchaseId, UpdatePurchaseEntry purchase, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchase, nameof(purchase));
            EnsureArg.IsNotDefault(purchaseId, nameof(purchaseId));
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            var existingPurchase = await ValidatePurchase(eventId, purchaseId, cancellationToken);

            existingPurchase.PatchFrom(purchase);
            var result = await _purchaseRepository.UpdatePurchaseAsync(existingPurchase, cancellationToken);

            return _mapper.Map<PurchaseShortEntry>(result);
        }

        /// <summary>
        /// Validate purchase
        /// </summary>
        private async Task<PurchaseEntry> ValidatePurchase(Guid eventId, Guid purchaseId, CancellationToken cancellationToken = default)
        {
            var existingPurchase = await _purchaseRepository.GetPurchaseAsync(eventId, purchaseId, cancellationToken);

            if (existingPurchase is null)
            {
                throw new EntityNotFoundException($"Purchase Id '{purchaseId}' not found");
            }

            if (existingPurchase.IsComplete)
            {
                throw new BadRequestException("Cannot edit completed purchase {PurchaseId}", purchaseId);
            }

            return existingPurchase;
        }
    }
}
