using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

        /// <summary>
        /// Получение списка покупок
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Response<PurchaseShortEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetPurchasesRoute)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAsync(
            int offset = 0,
            int limit = 10,
            string name = null,
            double? costMin = null,
            double? costMax = null,
            Guid? shoppingId = null,
            IEnumerable<string> purchaseTags = null,
            Guid? unitTypeId = null,
            bool? isActive = null)
        {
            var purchaseResponse = await _purchaseService.GetPurchasesAsync(offset, limit, name, costMin, costMax, shoppingId,
                                                                            purchaseTags, unitTypeId, isActive, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, purchaseResponse);
        }

        /// <summary>
        /// Получение данных о покупке по идентификатору
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(GetPurchaseEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetPurchaseRoute)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAsync(Guid purchaseId)
        {
            var purchaseEntry = await _purchaseService.GetPurchaseAsync(purchaseId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, purchaseEntry);
        }

        /// <summary>
        /// Добавление покупки
        /// </summary>
        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(PurchaseShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddPurchaseRoute)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostAsync([FromBody] UpsertPurchaseEntry addEntry)
        {
            var purchase = await _purchaseService.AddPurchaseAsync(addEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, purchase);
        }

        /// <summary>
        /// Удаление покупки
        /// </summary>
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeletePurchaseRoute)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteAsync(Guid purchaseId)
        {
            await _purchaseService.DeletePurchaseAsync(purchaseId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK);
        }

        /// <summary>
        /// Изменение данных о покупке
        /// </summary>
        [HttpPatch]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(PurchaseShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UpdatePurchaseRoute)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PatchAsync(Guid purchaseId, [FromBody] UpsertPurchaseEntry updateEntry)
        {
            var entry = await _purchaseService.UpdatePurchaseAsync(purchaseId, updateEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, entry);
        }
    }
}
