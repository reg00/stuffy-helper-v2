using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Api.Web;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.PurchaseUsage;
using System.Net;

namespace StuffyHelper.Api.Controllers
{
    [Authorize]
    public class PurchaseUsageController : Controller
    {
        private readonly IPurchaseUsageService _PurchaseUsageService;

        public PurchaseUsageController(IPurchaseUsageService PurchaseUsageService)
        {
            _PurchaseUsageService = PurchaseUsageService;
        }

        /// <summary>
        /// Получение списка того, какие участники какие продукты используют
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<PurchaseUsageShortEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetPurchaseUsagesRoute)]
        public async Task<IActionResult> GetAsync(
            int offset = 0,
            int limit = 10,
            Guid? eventId = null,
            Guid? participantId = null,
            Guid? purchaseId = null)
        {
            var purchaseUsageResponse = await _PurchaseUsageService.GetPurchaseUsagesAsync(offset, limit, eventId, participantId, purchaseId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, purchaseUsageResponse);
        }

        /// <summary>
        /// Получение данных об использовании участником продукта по идентификатору
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(GetPurchaseUsageEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetPurchaseUsageRoute)]
        public async Task<IActionResult> GetAsync(Guid purchaseUsageId)
        {
            var purchaseUsageEntry = await _PurchaseUsageService.GetPurchaseUsageAsync(purchaseUsageId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, purchaseUsageEntry);
        }

        /// <summary>
        /// Добавление использования участником продукта
        /// </summary>
        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(PurchaseUsageShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddPurchaseUsageRoute)]
        public async Task<IActionResult> PostAsync([FromBody] UpsertPurchaseUsageEntry addEntry)
        {
            var purchaseUsage = await _PurchaseUsageService.AddPurchaseUsageAsync(addEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, purchaseUsage);
        }

        /// <summary>
        /// Удаление использования участником продукта
        /// </summary>
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeletePurchaseUsageRoute)]
        public async Task<IActionResult> DeleteAsync(Guid purchaseUsageId)
        {
            await _PurchaseUsageService.DeletePurchaseUsageAsync(purchaseUsageId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK);
        }

        /// <summary>
        /// Узменение использования участником продукта (а надо ли??)
        /// </summary>
        [HttpPatch]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(PurchaseUsageShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UpdatePurchaseUsageRoute)]
        public async Task<IActionResult> PatchAsync(Guid purchaseUsageId, [FromBody] UpsertPurchaseUsageEntry updateEntry)
        {
            var entry = await _PurchaseUsageService.UpdatePurchaseUsageAsync(purchaseUsageId, updateEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, entry);
        }
    }
}
