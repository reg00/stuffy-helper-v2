using StuffyHelper.ApiGateway.Core.Services.Interfaces;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Clients.Interface;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services
{
    public class UnitTypeService : IUnitTypeService
    {
        private readonly IUnitTypeClient _unitTypeClient;

        public UnitTypeService(IUnitTypeClient unitTypeClient)
        {
            _unitTypeClient = unitTypeClient;
        }

        public async Task<GetUnitTypeEntry> GetUnitTypeAsync(string token, Guid unitTypeId, CancellationToken cancellationToken)
        {
            return await _unitTypeClient.GetUnitTypeAsync(token, unitTypeId, cancellationToken);
        }

        public async Task<Response<UnitTypeShortEntry>> GetUnitTypesAsync(
            string token, 
            int offset = 0,
            int limit = 10,
            string? name = null,
            Guid? purchaseId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default)
        {
            return await _unitTypeClient.GetUnitTypesAsync(token, offset, limit, name, purchaseId, isActive, cancellationToken);
        }

        public async Task<UnitTypeShortEntry> AddUnitTypeAsync(string token, UpsertUnitTypeEntry unitType, CancellationToken cancellationToken = default)
        {
            return await _unitTypeClient.CreateUnitTypeAsync(token, unitType, cancellationToken);
        }

        public async Task DeleteUnitTypeAsync(string token, Guid unitTypeId, CancellationToken cancellationToken = default)
        {
            await _unitTypeClient.DeleteUnitTypeAsync(token, unitTypeId, cancellationToken);
        }

        public async Task<UnitTypeShortEntry> UpdateUnitTypeAsync(string token, Guid unitTypeId, UpsertUnitTypeEntry unitType, CancellationToken cancellationToken = default)
        {
            return await _unitTypeClient.UpdateUnitTypeAsync(token, unitTypeId, unitType, cancellationToken);
        }
    }
}
