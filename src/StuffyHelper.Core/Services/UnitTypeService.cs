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
    public class UnitTypeService : IUnitTypeService
    {
        private readonly IUnitTypeRepository _unitTypeStore;
        private readonly IMapper _mapper;

        public UnitTypeService(IUnitTypeRepository unitTypeStore, IMapper mapper)
        {
            _unitTypeStore = unitTypeStore;
            _mapper = mapper;
        }

        public async Task<GetUnitTypeEntry> GetUnitTypeAsync(Guid unitTypeId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(unitTypeId, nameof(unitTypeId));

            var entry = await _unitTypeStore.GetUnitTypeAsync(unitTypeId, cancellationToken);

            return _mapper.Map<GetUnitTypeEntry>(entry);
        }

        public async Task<Response<UnitTypeShortEntry>> GetUnitTypesAsync(
            int offset = 0,
            int limit = 10,
            string? name = null,
            Guid? purchaseId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default)
        {
            var resp = await _unitTypeStore.GetUnitTypesAsync(offset, limit, name, purchaseId, isActive, cancellationToken);

            return new Response<UnitTypeShortEntry>()
            {
                Data = resp.Data.Select(x => _mapper.Map<UnitTypeShortEntry>(x)),
                TotalPages = resp.TotalPages,
                Total = resp.Total
            };
        }

        public async Task<UnitTypeShortEntry> AddUnitTypeAsync(UpsertUnitTypeEntry unitType, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(unitType, nameof(unitType));

            var entry = _mapper.Map<UnitTypeEntry>(unitType);
            var result = await _unitTypeStore.AddUnitTypeAsync(entry, cancellationToken);

            return _mapper.Map<UnitTypeShortEntry>(result);
        }

        public async Task DeleteUnitTypeAsync(Guid unitTypeId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(unitTypeId, nameof(unitTypeId));

            await _unitTypeStore.DeleteUnitTypeAsync(unitTypeId, cancellationToken);
        }

        public async Task<UnitTypeShortEntry> UpdateUnitTypeAsync(Guid unitTypeId, UpsertUnitTypeEntry unitType, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(unitType, nameof(unitType));
            EnsureArg.IsNotDefault(unitTypeId, nameof(unitTypeId));

            var existingUnitType = await _unitTypeStore.GetUnitTypeAsync(unitTypeId, cancellationToken);

            if (existingUnitType is null)
            {
                throw new EntityNotFoundException($"UnitType Id '{unitTypeId}' not found");
            }

            existingUnitType.PatchFrom(unitType);
            var result = await _unitTypeStore.UpdateUnitTypeAsync(existingUnitType, cancellationToken);

            return _mapper.Map<UnitTypeShortEntry>(result);
        }
    }
}
