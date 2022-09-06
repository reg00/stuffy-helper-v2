using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Api.Web;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.PurchaseTag;
using System.Net;

namespace StuffyHelper.Api.Controllers
{
    public class PurchaseTagController : Controller
    {
        private readonly IPurchaseTagService _purchaseTagService;

        public PurchaseTagController(IPurchaseTagService purchaseTagService)
        {
            _purchaseTagService = purchaseTagService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Response<GetPurchaseTagEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetPurchaseTagsRoute)]
        [Authorize]
        public async Task<IActionResult> GetAsync(
            int offset = 0,
            int limit = 10,
            string name = null,
            Guid? purchaseId = null,
            bool? isActive = null)
        {
            var purchaseTagResponse = await _purchaseTagService.GetPurchaseTagsAsync(offset, limit, name, purchaseId, isActive, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, purchaseTagResponse);
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetPurchaseTagEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetPurchaseTagRoute)]
        [Authorize]
        public async Task<IActionResult> GetAsync(Guid purchaseTagId)
        {
            var purchaseTagEntry = await _purchaseTagService.GetPurchaseTagAsync(purchaseTagId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, purchaseTagEntry);
        }

        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(GetPurchaseTagEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddPurchaseTagRoute)]
        [Authorize]
        public async Task<IActionResult> PostAsync([FromBody] UpsertPurchaseTagEntry addEntry)
        {
            var purchaseTag = await _purchaseTagService.AddPurchaseTagAsync(addEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, purchaseTag);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeletePurchaseTagRoute)]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(Guid purchaseTagId)
        {
            await _purchaseTagService.DeletePurchaseTagAsync(purchaseTagId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpPatch]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(GetPurchaseTagEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UpdatePurchaseTagRoute)]
        [Authorize]
        public async Task<IActionResult> PatchAsync(Guid purchaseTagId, [FromBody] UpsertPurchaseTagEntry updateEntry)
        {
            var entry = await _purchaseTagService.UpdatePurchaseTagAsync(purchaseTagId, updateEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, entry);
        }
    }

}
