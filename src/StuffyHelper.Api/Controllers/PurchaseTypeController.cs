using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Api.Web;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.PurchaseType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StuffyHelper.Api.Controllers
{
    public class PurchaseTypeController : Controller
    {
        private readonly IPurchaseTypeService _purchaseTypeService;

        public PurchaseTypeController(IPurchaseTypeService purchaseTypeService)
        {
            _purchaseTypeService = purchaseTypeService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Response<GetPurchaseTypeEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetPurchaseTypesRoute)]
        [Authorize]
        public async Task<IActionResult> GetAsync(
            int offset = 0,
            int limit = 10,
            string name = null,
            Guid? purchaseId = null,
            bool? isActive = null)
        {
            var purchaseTypeResponse = await _purchaseTypeService.GetPurchaseTypesAsync(offset, limit, name, purchaseId, isActive, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, purchaseTypeResponse);
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetPurchaseTypeEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetPurchaseTypeRoute)]
        [Authorize]
        public async Task<IActionResult> GetAsync(Guid purchaseTypeId)
        {
            var purchaseTypeEntry = await _purchaseTypeService.GetPurchaseTypeAsync(purchaseTypeId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, purchaseTypeEntry);
        }

        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(GetPurchaseTypeEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddPurchaseTypeRoute)]
        [Authorize]
        public async Task<IActionResult> PostAsync([FromBody] UpsertPurchaseTypeEntry addEntry)
        {
            var purchaseType = await _purchaseTypeService.AddPurchaseTypeAsync(addEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, purchaseType);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeletePurchaseTypeRoute)]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(Guid purchaseTypeId)
        {
            await _purchaseTypeService.DeletePurchaseTypeAsync(purchaseTypeId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpPatch]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(GetPurchaseTypeEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UpdatePurchaseTypeRoute)]
        [Authorize]
        public async Task<IActionResult> PatchAsync(Guid purchaseTypeId, [FromBody] UpsertPurchaseTypeEntry updateEntry)
        {
            var entry = await _purchaseTypeService.UpdatePurchaseTypeAsync(purchaseTypeId, updateEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, entry);
        }
    }

}
