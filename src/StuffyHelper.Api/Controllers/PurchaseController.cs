using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Api.Web;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Purchase;
using System.Net;

namespace StuffyHelper.Api.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly IPurchaseService _purchaseService;

        public PurchaseController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Response<GetPurchaseEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetPurchasesRoute)]
        [Authorize]
        public async Task<IActionResult> GetAsync(
            int offset = 0,
            int limit = 10,
            string name = null,
            int? countMin = null,
            int? countMax = null,
            double? costMin = null,
            double? costMax = null,
            double? weightMin = null,
            double? weightMax = null,
            Guid? shoppingId = null,
            Guid? purchaseTypeId = null,
            Guid? unitTypeId = null,
            bool? isActive = null)
        {
            var purchaseResponse = await _purchaseService.GetPurchasesAsync(offset, limit, name, countMin, countMax, costMin, costMax, weightMin, weightMax,
                                                                            shoppingId, purchaseTypeId, unitTypeId, isActive, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, purchaseResponse);
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetPurchaseEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetPurchaseRoute)]
        [Authorize]
        public async Task<IActionResult> GetAsync(Guid purchaseId)
        {
            var purchaseEntry = await _purchaseService.GetPurchaseAsync(purchaseId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, purchaseEntry);
        }

        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(GetPurchaseEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddPurchaseRoute)]
        [Authorize]
        public async Task<IActionResult> PostAsync([FromBody] UpsertPurchaseEntry addEntry)
        {
            var purchase = await _purchaseService.AddPurchaseAsync(addEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, purchase);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeletePurchaseRoute)]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(Guid purchaseId)
        {
            await _purchaseService.DeletePurchaseAsync(purchaseId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpPatch]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(GetPurchaseEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UpdatePurchaseRoute)]
        [Authorize]
        public async Task<IActionResult> PatchAsync(Guid purchaseId, [FromBody] UpsertPurchaseEntry updateEntry)
        {
            var entry = await _purchaseService.UpdatePurchaseAsync(purchaseId, updateEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, entry);
        }
    }
}
