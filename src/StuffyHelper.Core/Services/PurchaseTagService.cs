using AutoMapper;
using EnsureThat;
using Reg00.Infrastructure.Errors;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Models;
using StuffyHelper.Core.Services.Interfaces;
using StuffyHelper.Data.Repository.Interfaces;

namespace StuffyHelper.Core.Services
{
    /// <inheritdoc />
    public class PurchaseTagService : IPurchaseTagService
    {
        private readonly IPurchaseTagRepository _purchaseTagRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Ctor.
        /// </summary>
        public PurchaseTagService(IPurchaseTagRepository purchaseTagRepository, IMapper mapper)
        {
            _purchaseTagRepository = purchaseTagRepository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<GetPurchaseTagEntry> GetPurchaseTagAsync(Guid purchaseTagId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(purchaseTagId, nameof(purchaseTagId));

            var entry = await _purchaseTagRepository.GetPurchaseTagAsync(purchaseTagId, cancellationToken);

            return _mapper.Map<GetPurchaseTagEntry>(entry);
        }

        /// <inheritdoc />
        public async Task<Response<PurchaseTagShortEntry>> GetPurchaseTagsAsync(
            int offset = 0,
            int limit = 10,
            string? name = null,
            Guid? purchaseId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default)
        {
            var resp = await _purchaseTagRepository.GetPurchaseTagsAsync(offset, limit, name, purchaseId, isActive, cancellationToken);

            return new Response<PurchaseTagShortEntry>()
            {
                Data = resp.Data.Select(x => _mapper.Map<PurchaseTagShortEntry>(x)),
                TotalPages = resp.TotalPages,
                Total = resp.Total
            };
        }

        /// <inheritdoc />
        public async Task<PurchaseTagShortEntry> AddPurchaseTagAsync(UpsertPurchaseTagEntry purchaseTag, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchaseTag, nameof(purchaseTag));

            var entry = _mapper.Map<PurchaseTagEntry>(purchaseTag);
            var result = await _purchaseTagRepository.AddPurchaseTagAsync(entry, cancellationToken);

            return _mapper.Map<PurchaseTagShortEntry>(result);
        }

        /// <inheritdoc />
        public async Task DeletePurchaseTagAsync(Guid purchaseTagId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(purchaseTagId, nameof(purchaseTagId));

            await _purchaseTagRepository.DeletePurchaseTagAsync(purchaseTagId, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<PurchaseTagShortEntry> UpdatePurchaseTagAsync(Guid purchaseTagId, UpsertPurchaseTagEntry purchaseTag, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(purchaseTag, nameof(purchaseTag));
            EnsureArg.IsNotDefault(purchaseTagId, nameof(purchaseTagId));

            var existingPurchaseTag = await _purchaseTagRepository.GetPurchaseTagAsync(purchaseTagId, cancellationToken);

            if (existingPurchaseTag is null)
            {
                throw new EntityNotFoundException($"PurchaseTag Id '{purchaseTagId}' not found");
            }

            existingPurchaseTag.PatchFrom(purchaseTag);
            var result = await _purchaseTagRepository.UpdatePurchaseTagAsync(existingPurchaseTag, cancellationToken);

            return _mapper.Map<PurchaseTagShortEntry>(result);
        }
    }
}
