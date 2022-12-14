using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Api.Web;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Shopping;
using System.Net;

namespace StuffyHelper.Api.Controllers
{
    [Authorize]
    public class ShoppingController : Controller
    {
        private readonly IShoppingService _shoppingService;

        public ShoppingController(IShoppingService shoppingService)
        {
            _shoppingService = shoppingService;
        }

        /// <summary>
        /// Получение списка походов в магазин
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Response<ShoppingShortEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetShoppingsRoute)]
        public async Task<IActionResult> GetAsync(
            int offset = 0,
            int limit = 10,
            DateTime? shoppingDateStart = null,
            DateTime? shoppingDateEnd = null,
            Guid? participantId = null,
            Guid? eventId = null,
            string? description = null)
        {
            var shoppingResponse = await _shoppingService.GetShoppingsAsync(offset, limit, shoppingDateStart, shoppingDateEnd,
                                                                            participantId, eventId, description, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, shoppingResponse);
        }

        /// <summary>
        /// Получение информации о походе в магазин по идентификатору
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(GetShoppingEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetShoppingRoute)]
        public async Task<IActionResult> GetAsync(Guid shoppingId)
        {
            var shoppingEntry = await _shoppingService.GetShoppingAsync(shoppingId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, shoppingEntry);
        }

        /// <summary>
        /// Добавление похода в магазин
        /// </summary>
        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(ShoppingShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddShoppingRoute)]
        public async Task<IActionResult> PostAsync([FromBody] AddShoppingEntry addEntry)
        {
            var shopping = await _shoppingService.AddShoppingAsync(addEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, shopping);
        }

        /// <summary>
        /// Удаление похода в магазин
        /// </summary>
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeleteShoppingRoute)]
        public async Task<IActionResult> DeleteAsync(Guid shoppingId)
        {
            await _shoppingService.DeleteShoppingAsync(shoppingId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK);
        }

        /// <summary>
        /// Изменение информации о походе в магазин
        /// </summary>
        [HttpPatch]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(ShoppingShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UpdateShoppingRoute)]
        public async Task<IActionResult> PatchAsync(Guid shoppingId, [FromBody] UpdateShoppingEntry updateEntry)
        {
            var entry = await _shoppingService.UpdateShoppingAsync(shoppingId, updateEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, entry);
        }
    }
}
