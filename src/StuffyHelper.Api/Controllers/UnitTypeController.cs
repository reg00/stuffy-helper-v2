using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Api.Web;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.UnitType;
using System.Net;

namespace StuffyHelper.Api.Controllers
{
    public class UnitTypeController : Controller
    {
        private readonly IUnitTypeService _unitTypeService;

        public UnitTypeController(IUnitTypeService unitTypeService)
        {
            _unitTypeService = unitTypeService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Response<GetUnitTypeEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetUnitTypesRoute)]
        [Authorize]
        public async Task<IActionResult> GetAsync(
            int offset = 0,
            int limit = 10,
            string name = null,
            Guid? purchaseId = null,
            bool? isActive = null)
        {
            var unitTypeResponse = await _unitTypeService.GetUnitTypesAsync(offset, limit, name, purchaseId, isActive, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, unitTypeResponse);
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetUnitTypeEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetUnitTypeRoute)]
        [Authorize]
        public async Task<IActionResult> GetAsync(Guid unitTypeId)
        {
            var unitTypeEntry = await _unitTypeService.GetUnitTypeAsync(unitTypeId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, unitTypeEntry);
        }

        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(GetUnitTypeEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddUnitTypeRoute)]
        [Authorize]
        public async Task<IActionResult> PostAsync([FromBody] UpsertUnitTypeEntry addEntry)
        {
            var unitType = await _unitTypeService.AddUnitTypeAsync(addEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, unitType);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeleteUnitTypeRoute)]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(Guid unitTypeId)
        {
            await _unitTypeService.DeleteUnitTypeAsync(unitTypeId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpPatch]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(GetUnitTypeEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UpdateUnitTypeRoute)]
        [Authorize]
        public async Task<IActionResult> PatchAsync(Guid unitTypeId, [FromBody] UpsertUnitTypeEntry updateEntry)
        {
            var entry = await _unitTypeService.UpdateUnitTypeAsync(unitTypeId, updateEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, entry);
        }
    }

}
