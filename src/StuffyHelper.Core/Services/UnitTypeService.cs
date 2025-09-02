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
    public class UnitTypeService : IUnitTypeService
    {
        private readonly IUnitTypeRepository _unitTypeRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Ctor.
        /// </summary>
        public UnitTypeService(IUnitTypeRepository unitTypeRepository, IMapper mapper)
        {
            _unitTypeRepository = unitTypeRepository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<GetUnitTypeEntry> GetUnitTypeAsync(Guid unitTypeId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(unitTypeId, nameof(unitTypeId));

            var entry = await _unitTypeRepository.GetUnitTypeAsync(unitTypeId, cancellationToken);

            return _mapper.Map<GetUnitTypeEntry>(entry);
        }

        /// <inheritdoc />
        public async Task<Response<UnitTypeShortEntry>> GetUnitTypesAsync(
            int offset = 0,
            int limit = 10,
            string? name = null,
            Guid? purchaseId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default)
        {
            var resp = await _unitTypeRepository.GetUnitTypesAsync(offset, limit, name, purchaseId, isActive, cancellationToken);

            return new Response<UnitTypeShortEntry>()
            {
                Data = resp.Data.Select(x => _mapper.Map<UnitTypeShortEntry>(x)),
                TotalPages = resp.TotalPages,
                Total = resp.Total
            };
        }

        /// <inheritdoc />
        public async Task<UnitTypeShortEntry> AddUnitTypeAsync(UpsertUnitTypeEntry unitType, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(unitType, nameof(unitType));

            var entry = _mapper.Map<UnitTypeEntry>(unitType);
            var result = await _unitTypeRepository.AddUnitTypeAsync(entry, cancellationToken);

            return _mapper.Map<UnitTypeShortEntry>(result);
        }

        /// <inheritdoc />
        public async Task DeleteUnitTypeAsync(Guid unitTypeId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(unitTypeId, nameof(unitTypeId));

            await _unitTypeRepository.DeleteUnitTypeAsync(unitTypeId, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<UnitTypeShortEntry> UpdateUnitTypeAsync(Guid unitTypeId, UpsertUnitTypeEntry unitType, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(unitType, nameof(unitType));
            EnsureArg.IsNotDefault(unitTypeId, nameof(unitTypeId));

            var existingUnitType = await _unitTypeRepository.GetUnitTypeAsync(unitTypeId, cancellationToken);

            if (existingUnitType is null)
            {
                throw new EntityNotFoundException($"UnitType Id '{unitTypeId}' not found");
            }

            existingUnitType.PatchFrom(unitType);
            var result = await _unitTypeRepository.UpdateUnitTypeAsync(existingUnitType, cancellationToken);

            return _mapper.Map<UnitTypeShortEntry>(result);
        }
    }
}
