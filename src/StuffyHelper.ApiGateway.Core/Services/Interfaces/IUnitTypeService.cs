using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services.Interfaces
{
    public interface IUnitTypeService
    {
        Task<GetUnitTypeEntry> GetUnitTypeAsync(string token, Guid unitTypeId, CancellationToken cancellationToken);

        Task<Response<UnitTypeShortEntry>> GetUnitTypesAsync(
            string token,
            int offset = 0,
            int limit = 10,
            string? name = null,
            Guid? purchaseId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default);

        Task<UnitTypeShortEntry> AddUnitTypeAsync(string token, UpsertUnitTypeEntry unitType, CancellationToken cancellationToken = default);

        Task DeleteUnitTypeAsync(string token, Guid unitTypeId, CancellationToken cancellationToken = default);

        Task<UnitTypeShortEntry> UpdateUnitTypeAsync(string token, Guid unitTypeId, UpsertUnitTypeEntry unitType, CancellationToken cancellationToken = default);
    }
}
