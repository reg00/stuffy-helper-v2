using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Api.Web;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Shopping;
using System.Net;

namespace StuffyHelper.Api.Controllers
{
    public class ShoppingController : Controller
    {
        private readonly IShoppingService _shoppingService;

        public ShoppingController(IShoppingService shoppingService)
        {
            _shoppingService = shoppingService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Response<GetShoppingEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetShoppingsRoute)]
        [Authorize]
        public async Task<IActionResult> GetAsync(
            int offset = 0,
            int limit = 10,
            DateTime? shoppingDateStart = null,
            DateTime? shoppingDateEnd = null,
            Guid? participantId = null,
            Guid? eventId = null,
            string description = null,
            bool? isActive = null)
        {
            var shoppingResponse = await _shoppingService.GetShoppingsAsync(offset, limit, shoppingDateStart, shoppingDateEnd,
                                                                            participantId, eventId, description, isActive, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, shoppingResponse);
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetShoppingEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetShoppingRoute)]
        [Authorize]
        public async Task<IActionResult> GetAsync(Guid shoppingId)
        {
            var shoppingEntry = await _shoppingService.GetShoppingAsync(shoppingId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, shoppingEntry);
        }

        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(GetShoppingEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddShoppingRoute)]
        [Authorize]
        public async Task<IActionResult> PostAsync([FromBody] UpsertShoppingEntry addEntry)
        {
            var shopping = await _shoppingService.AddShoppingAsync(addEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, shopping);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeleteShoppingRoute)]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(Guid shoppingId)
        {
            await _shoppingService.DeleteShoppingAsync(shoppingId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpPatch]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(GetShoppingEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UpdateShoppingRoute)]
        [Authorize]
        public async Task<IActionResult> PatchAsync(Guid shoppingId, [FromBody] UpsertShoppingEntry updateEntry)
        {
            var entry = await _shoppingService.UpdateShoppingAsync(shoppingId, updateEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, entry);
        }
    }
}
