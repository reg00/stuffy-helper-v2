using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.UnitType
{
    public interface IUnitTypeService
    {
        Task<GetUnitTypeEntry> GetUnitTypeAsync(Guid unitTypeId, CancellationToken cancellationToken);

        Task<Response<GetUnitTypeEntry>> GetUnitTypesAsync(
            int offset = 0,
            int limit = 10,
            string name = null,
            Guid? purchaseId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default);

        Task<GetUnitTypeEntry> AddUnitTypeAsync(UpsertUnitTypeEntry unitType, CancellationToken cancellationToken = default);

        Task DeleteUnitTypeAsync(Guid unitTypeId, CancellationToken cancellationToken = default);

        Task<GetUnitTypeEntry> UpdateUnitTypeAsync(Guid unitTypeId, UpsertUnitTypeEntry unitType, CancellationToken cancellationToken = default);
}
}
