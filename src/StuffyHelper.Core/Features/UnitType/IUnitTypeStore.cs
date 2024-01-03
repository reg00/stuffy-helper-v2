using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.UnitType
{
    public interface IUnitTypeStore
    {
        Task<UnitTypeEntry> GetUnitTypeAsync(Guid unitTypeId, CancellationToken cancellationToken);

        Task<PagedData<UnitTypeEntry>> GetUnitTypesAsync(
            int offset = 0,
            int limit = 10,
            string? name = null,
            Guid? purchaseId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default);

        Task<UnitTypeEntry> AddUnitTypeAsync(UnitTypeEntry unitType, CancellationToken cancellationToken = default);

        Task DeleteUnitTypeAsync(Guid unitTypeId, CancellationToken cancellationToken = default);

        Task<UnitTypeEntry> UpdateUnitTypeAsync(UnitTypeEntry unitType, CancellationToken cancellationToken = default);
    }
}
