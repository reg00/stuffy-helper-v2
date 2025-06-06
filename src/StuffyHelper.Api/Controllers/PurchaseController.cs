using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;
using StuffyHelper.Contracts.Models;
using StuffyHelper.Core.Services.Interfaces;

namespace StuffyHelper.Api.Controllers
{
    [Authorize]
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
        [ProducesResponseType(typeof(Response<GetPurchaseEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetPurchasesRoute)]
        public async Task<IActionResult> GetAsync(
            int offset = 0,
            int limit = 10,
            string? name = null,
            double? costMin = null,
            double? costMax = null,
            Guid? eventId = null,
            IEnumerable<string>? purchaseTags = null,
            Guid? unitTypeId = null,
            bool? isComplete = null)
        {
            var purchaseResponse = await _purchaseService.GetPurchasesAsync(offset, limit, name, costMin, costMax, eventId,
                                                                            purchaseTags, unitTypeId, isComplete, HttpContext.RequestAborted);

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
        public async Task<IActionResult> PostAsync([FromBody] AddPurchaseEntry addEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse(ModelState));
            }

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
        public async Task<IActionResult> PatchAsync(Guid purchaseId, [FromBody] UpdatePurchaseEntry updateEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse(ModelState));
            }

            var entry = await _purchaseService.UpdatePurchaseAsync(purchaseId, updateEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, entry);
        }
    }
}
