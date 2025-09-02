using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services.Interfaces
{
    /// <summary>
    /// Interface for work with unit types in API Gateway
    /// </summary>
    public interface IUnitTypeService
    {
        /// <summary>
        /// Get unit type by identifier
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="unitTypeId">Unit type identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Detailed unit type information</returns>
        Task<GetUnitTypeEntry> GetUnitTypeAsync(string token, Guid unitTypeId, CancellationToken cancellationToken);

        /// <summary>
        /// Get filtered list of unit types
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="offset">Number of records to skip</param>
        /// <param name="limit">Maximum number of records to return</param>
        /// <param name="name">Unit type name filter</param>
        /// <param name="purchaseId">Purchase identifier filter</param>
        /// <param name="isActive">Unit type activity status filter</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of unit types</returns>
        Task<Response<UnitTypeShortEntry>> GetUnitTypesAsync(
            string token,
            int offset = 0,
            int limit = 10,
            string? name = null,
            Guid? purchaseId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Add new unit type
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="unitType">Unit type data to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created unit type information</returns>
        Task<UnitTypeShortEntry> AddUnitTypeAsync(string token, UpsertUnitTypeEntry unitType, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete unit type by identifier
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="unitTypeId">Unit type identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeleteUnitTypeAsync(string token, Guid unitTypeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update unit type information
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="unitTypeId">Unit type identifier</param>
        /// <param name="unitType">Updated unit type data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated unit type information</returns>
        Task<UnitTypeShortEntry> UpdateUnitTypeAsync(string token, Guid unitTypeId, UpsertUnitTypeEntry unitType, CancellationToken cancellationToken = default);
    }
}