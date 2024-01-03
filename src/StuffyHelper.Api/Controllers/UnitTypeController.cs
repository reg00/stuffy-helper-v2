using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Api.Features.Models.Request.UnitType;
using StuffyHelper.Api.Web;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.UnitType;
using System.Net;

namespace StuffyHelper.Api.Controllers
{
    [Authorize]
    public class UnitTypeController : ApiControllerBase
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
        [ProducesResponseType(typeof(PagedData<UnitTypeShortEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetUnitTypesRoute)]
        public async Task<Result<PagedData<UnitTypeShortEntry>>> GetAsync(
            int offset = 0,
            int limit = 10,
            string? name = null,
            Guid? purchaseId = null,
            bool? isActive = null)
        {
            var result = await Mediator.Send(new GetUnitTypeListRequest(offset, limit, name, purchaseId, isActive), HttpContext.RequestAborted);

            return result;
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
        public async Task<IActionResult> GetAsync(Guid unitTypeId)
        {
            var unitTypeEntry = await _unitTypeService.GetUnitTypeAsync(unitTypeId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, unitTypeEntry);
        }

        /// <summary>
        /// Добавление единицы измерения
        /// </summary>
        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(UnitTypeShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddUnitTypeRoute)]
        public async Task<IActionResult> PostAsync([FromBody] UpsertUnitTypeEntry addEntry)
        {
            var unitType = await _unitTypeService.AddUnitTypeAsync(addEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, unitType);
        }

        /// <summary>
        /// Удаление единицы измерения
        /// </summary>
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeleteUnitTypeRoute)]
        public async Task<IActionResult> DeleteAsync(Guid unitTypeId)
        {
            await _unitTypeService.DeleteUnitTypeAsync(unitTypeId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK);
        }

        /// <summary>
        /// Изменение единицы измерения
        /// </summary>
        [HttpPatch]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(UnitTypeShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UpdateUnitTypeRoute)]
        public async Task<IActionResult> PatchAsync(Guid unitTypeId, [FromBody] UpsertUnitTypeEntry updateEntry)
        {
            var entry = await _unitTypeService.UpdateUnitTypeAsync(unitTypeId, updateEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, entry);
        }
    }

}
