using Microsoft.AspNetCore.Mvc;
using System.Net;
using StuffyHelper.ApiGateway.Core.Services.Interfaces;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Controllers
{
    public class PurchaseTagController : AuthorizedApiController
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
        public async Task<Response<PurchaseTagShortEntry>> GetAsync(
            int offset = 0,
            int limit = 10,
            string? name = null,
            Guid? purchaseId = null,
            bool? isActive = null)
        {
            return await _purchaseTagService.GetPurchaseTagsAsync(Token, offset, limit, name, purchaseId, isActive, HttpContext.RequestAborted);
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
        public async Task<GetPurchaseTagEntry> GetAsync(Guid purchaseTagId)
        {
            return await _purchaseTagService.GetPurchaseTagAsync(Token, purchaseTagId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Добавление тэга покупки
        /// </summary>
        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(PurchaseTagShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddPurchaseTagRoute)]
        public async Task<PurchaseTagShortEntry> PostAsync([FromBody] UpsertPurchaseTagEntry addEntry)
        {
            return await _purchaseTagService.AddPurchaseTagAsync(Token, addEntry, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Удаление тэга покупки
        /// </summary>
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeletePurchaseTagRoute)]
        public async Task DeleteAsync(Guid purchaseTagId)
        {
            await _purchaseTagService.DeletePurchaseTagAsync(Token, purchaseTagId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Изменение тэга покупки
        /// </summary>
        [HttpPatch]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(PurchaseTagShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UpdatePurchaseTagRoute)]
        public async Task<PurchaseTagShortEntry> PatchAsync(Guid purchaseTagId, [FromBody] UpsertPurchaseTagEntry updateEntry)
        {
            return await _purchaseTagService.UpdatePurchaseTagAsync(Token, purchaseTagId, updateEntry, HttpContext.RequestAborted);
        }
    }

}
