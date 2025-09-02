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
        private readonly IPurchaseTagPipeline _purchaseTagPipeline;
        private readonly IMapper _mapper;

        /// <summary>
        /// Ctor.
        /// </summary>
        public PurchaseService(
            IPurchaseRepository purchaseRepository,
            IPurchaseTagPipeline purchaseTagPipeline,
            IMapper mapper)
        {
            _purchaseRepository = purchaseRepository;
            _purchaseTagPipeline = purchaseTagPipeline;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<GetPurchaseEntry> GetPurchaseAsync(Guid purchaseId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(purchaseId, nameof(purchaseId));

            var entry = await _purchaseRepository.GetPurchaseAsync(purchaseId, cancellationToken);

            return _mapper.Map<GetPurchaseEntry>(entry);
        }

        /// <inheritdoc />
        public async Task<Response<GetPurchaseEntry>> GetPurchasesAsync(
            int offset = 0,
            int limit = 10,
            string? name = null,
            double? costMin = null,
            double? costMax = null,
            Guid? eventId = null,
            IEnumerable<string>? purchaseTags = null,
            Guid? unitTypeId = null,
            bool? isComplete = null,
            CancellationToken cancellationToken = default)
        {
            var resp = await _purchaseRepository.GetPurchasesAsync(offset, limit, name, costMin, costMax,
                                                              eventId, purchaseTags, unitTypeId, isComplete, cancellationToken);

            return new Response<GetPurchaseEntry>()
            {
                Data = resp.Data.Select(x => _mapper.Map<GetPurchaseEntry>(x)),
                TotalPages = resp.TotalPages,
                Total = resp.Total
            };
        }

        /// <inheritdoc />
        public async Task<PurchaseShortEntry> AddPurchaseAsync(AddPurchaseEntry purchase, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchase, nameof(purchase));

            var entry = _mapper.Map<PurchaseEntry>(purchase);
            await _purchaseTagPipeline.ProcessAsync(entry, purchase.PurchaseTags, cancellationToken);
            var result = await _purchaseRepository.AddPurchaseAsync(entry, cancellationToken);

            return _mapper.Map<PurchaseShortEntry>(result);
        }

        /// <inheritdoc />
        public async Task DeletePurchaseAsync(Guid purchaseId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(purchaseId, nameof(purchaseId));

            await ValidatePurchase(purchaseId, cancellationToken);

            await _purchaseRepository.DeletePurchaseAsync(purchaseId, cancellationToken);
        }

        /// <inheritdoc />
        public async Task CompletePurchaseAsync(Guid purchaseId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(purchaseId, nameof(purchaseId));

            await ValidatePurchase(purchaseId, cancellationToken);

            await _purchaseRepository.CompletePurchaseAsync(purchaseId, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<PurchaseShortEntry> UpdatePurchaseAsync(Guid purchaseId, UpdatePurchaseEntry purchase, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchase, nameof(purchase));
            EnsureArg.IsNotDefault(purchaseId, nameof(purchaseId));

            var existingPurchase = await ValidatePurchase(purchaseId, cancellationToken);

            existingPurchase.PatchFrom(purchase);
            await _purchaseTagPipeline.ProcessAsync(existingPurchase, purchase.PurchaseTags, cancellationToken);
            var result = await _purchaseRepository.UpdatePurchaseAsync(existingPurchase, cancellationToken);

            return _mapper.Map<PurchaseShortEntry>(result);
        }

        /// <summary>
        /// Validate purchase
        /// </summary>
        private async Task<PurchaseEntry> ValidatePurchase(Guid purchaseId, CancellationToken cancellationToken = default)
        {
            var existingPurchase = await _purchaseRepository.GetPurchaseAsync(purchaseId, cancellationToken);

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
