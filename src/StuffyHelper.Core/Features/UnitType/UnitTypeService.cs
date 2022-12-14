using EnsureThat;
using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.UnitType
{
    public class UnitTypeService : IUnitTypeService
    {
        private readonly IUnitTypeStore _unitTypeStore;

        public UnitTypeService(IUnitTypeStore UnitTypeStore)
        {
            _unitTypeStore = UnitTypeStore;
        }

        public async Task<GetUnitTypeEntry> GetUnitTypeAsync(Guid unitTypeId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(unitTypeId, nameof(unitTypeId));

            var entry = await _unitTypeStore.GetUnitTypeAsync(unitTypeId, cancellationToken);

            return new GetUnitTypeEntry(entry);
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
                Data = resp.Data.Select(x => new UnitTypeShortEntry(x)),
                TotalPages = resp.TotalPages,
                Total = resp.Total
            };
        }

        public async Task<UnitTypeShortEntry> AddUnitTypeAsync(UpsertUnitTypeEntry unitType, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(unitType, nameof(unitType));

            var entry = unitType.ToCommonEntry();
            var result = await _unitTypeStore.AddUnitTypeAsync(entry, cancellationToken);

            return new UnitTypeShortEntry(result);
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
                throw new ResourceNotFoundException($"UnitType Id '{unitTypeId}' not found");
            }

            existingUnitType.PatchFrom(unitType);
            var result = await _unitTypeStore.UpdateUnitTypeAsync(existingUnitType, cancellationToken);

            return new UnitTypeShortEntry(result);
        }
    }
}
