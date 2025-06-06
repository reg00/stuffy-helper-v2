using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Data.Repository.Interfaces
{
    public interface IUnitTypeRepository
    {
        Task<UnitTypeEntry> GetUnitTypeAsync(Guid unitTypeId, CancellationToken cancellationToken);

        Task<Response<UnitTypeEntry>> GetUnitTypesAsync(
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
