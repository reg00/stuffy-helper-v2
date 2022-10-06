using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Api.Web;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.PurchaseUsage;
using System.Net;

namespace StuffyHelper.Api.Controllers
{
    public class PurchaseUsageController : Controller
    {
        private readonly IPurchaseUsageService _PurchaseUsageService;

        public PurchaseUsageController(IPurchaseUsageService PurchaseUsageService)
        {
            _PurchaseUsageService = PurchaseUsageService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Response<GetPurchaseUsageEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetPurchaseUsagesRoute)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAsync(
            int offset = 0,
            int limit = 10,
            Guid? participantId = null,
            Guid? purchaseId = null)
        {
            var purchaseUsageResponse = await _PurchaseUsageService.GetPurchaseUsagesAsync(offset, limit, participantId, purchaseId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, purchaseUsageResponse);
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetPurchaseUsageEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetPurchaseUsageRoute)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAsync(Guid purchaseUsageId)
        {
            var purchaseUsageEntry = await _PurchaseUsageService.GetPurchaseUsageAsync(purchaseUsageId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, purchaseUsageEntry);
        }

        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(GetPurchaseUsageEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddPurchaseUsageRoute)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostAsync([FromBody] UpsertPurchaseUsageEntry addEntry)
        {
            var purchaseUsage = await _PurchaseUsageService.AddPurchaseUsageAsync(addEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, purchaseUsage);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeletePurchaseUsageRoute)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteAsync(Guid purchaseUsageId)
        {
            await _PurchaseUsageService.DeletePurchaseUsageAsync(purchaseUsageId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpPatch]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(GetPurchaseUsageEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UpdatePurchaseUsageRoute)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PatchAsync(Guid purchaseUsageId, [FromBody] UpsertPurchaseUsageEntry updateEntry)
        {
            var entry = await _PurchaseUsageService.UpdatePurchaseUsageAsync(purchaseUsageId, updateEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, entry);
        }
    }
}
