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
    public class PurchaseTagController : Controller
    {
        private readonly IPurchaseTagService _purchaseTagService;

        public PurchaseTagController(IPurchaseTagService purchaseTagService)
        {
            _purchaseTagService = purchaseTagService;
        }

        /// <summary>
        /// Получение списка тэгов покупок
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Response<PurchaseTagShortEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetPurchaseTagsRoute)]
        public async Task<IActionResult> GetAsync(
            int offset = 0,
            int limit = 10,
            string? name = null,
            Guid? purchaseId = null,
            bool? isActive = null)
        {
            var purchaseTagResponse = await _purchaseTagService.GetPurchaseTagsAsync(offset, limit, name, purchaseId, isActive, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, purchaseTagResponse);
        }

        /// <summary>
        /// Получение данных тэга покупки
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(GetPurchaseTagEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetPurchaseTagRoute)]
        public async Task<IActionResult> GetAsync(Guid purchaseTagId)
        {
            var purchaseTagEntry = await _purchaseTagService.GetPurchaseTagAsync(purchaseTagId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, purchaseTagEntry);
        }

        /// <summary>
        /// Добавление тэга покупки
        /// </summary>
        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(PurchaseTagShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddPurchaseTagRoute)]
        public async Task<IActionResult> PostAsync([FromBody] UpsertPurchaseTagEntry addEntry)
        {
            var purchaseTag = await _purchaseTagService.AddPurchaseTagAsync(addEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, purchaseTag);
        }

        /// <summary>
        /// Удаление тэга покупки
        /// </summary>
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeletePurchaseTagRoute)]
        public async Task<IActionResult> DeleteAsync(Guid purchaseTagId)
        {
            await _purchaseTagService.DeletePurchaseTagAsync(purchaseTagId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK);
        }

        /// <summary>
        /// Изменение тэга покупки
        /// </summary>
        [HttpPatch]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(PurchaseTagShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UpdatePurchaseTagRoute)]
        public async Task<IActionResult> PatchAsync(Guid purchaseTagId, [FromBody] UpsertPurchaseTagEntry updateEntry)
        {
            var entry = await _purchaseTagService.UpdatePurchaseTagAsync(purchaseTagId, updateEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, entry);
        }
    }

}
