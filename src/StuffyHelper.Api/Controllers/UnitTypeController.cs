using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;
using StuffyHelper.Contracts.Models;
using StuffyHelper.Core.Services.Interfaces;

namespace StuffyHelper.Api.Controllers
{
    /// <summary>
    /// Удиницы измерения
    /// </summary>
    [Authorize]
    public class UnitTypeController : Controller
    {
        private readonly IUnitTypeService _unitTypeService;

        public UnitTypeController(IUnitTypeService unitTypeService)
        {
            _unitTypeService = unitTypeService;
        }

        /// <summary>
        /// Получение списка единиц измерения
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Response<UnitTypeShortEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetUnitTypesRoute)]
        public async Task<Response<UnitTypeShortEntry>> GetAsync(
            int offset = 0,
            int limit = 10,
            string? name = null,
            Guid? purchaseId = null,
            bool? isActive = null)
        {
            return await _unitTypeService.GetUnitTypesAsync(offset, limit, name, purchaseId, isActive, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Получение информации о единице измерения по идентификатору
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(GetUnitTypeEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetUnitTypeRoute)]
        public async Task<GetUnitTypeEntry> GetAsync(Guid unitTypeId)
        {
            return await _unitTypeService.GetUnitTypeAsync(unitTypeId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Добавление единицы измерения
        /// </summary>
        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(UnitTypeShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddUnitTypeRoute)]
        public async Task<UnitTypeShortEntry> PostAsync([FromBody] UpsertUnitTypeEntry addEntry)
        {
            return await _unitTypeService.AddUnitTypeAsync(addEntry, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Удаление единицы измерения
        /// </summary>
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeleteUnitTypeRoute)]
        public async Task DeleteAsync(Guid unitTypeId)
        {
            await _unitTypeService.DeleteUnitTypeAsync(unitTypeId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Изменение единицы измерения
        /// </summary>
        [HttpPatch]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(UnitTypeShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UpdateUnitTypeRoute)]
        public async Task<UnitTypeShortEntry> PatchAsync(Guid unitTypeId, [FromBody] UpsertUnitTypeEntry updateEntry)
        {
            return await _unitTypeService.UpdateUnitTypeAsync(unitTypeId, updateEntry, HttpContext.RequestAborted);
        }
    }

}
