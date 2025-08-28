using Microsoft.AspNetCore.Mvc;
using System.Net;
using StuffyHelper.ApiGateway.Core.Services.Interfaces;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Controllers
{
    public class PurchaseUsageController : AuthorizedApiController
    {
        private readonly IPurchaseUsageService _purchaseUsageService;

        public PurchaseUsageController(IPurchaseUsageService purchaseUsageService)
        {
            _purchaseUsageService = purchaseUsageService;
        }

        /// <summary>
        /// Получение списка того, какие участники какие продукты используют
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Response<PurchaseUsageShortEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetPurchaseUsagesRoute)]
        public async Task<Response<PurchaseUsageShortEntry>> GetAsync(
            int offset = 0,
            int limit = 10,
            Guid? eventId = null,
            Guid? participantId = null,
            Guid? purchaseId = null)
        {
            return await _purchaseUsageService.GetPurchaseUsagesAsync(Token, offset, limit, eventId, participantId, purchaseId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Получение данных об использовании участником продукта по идентификатору
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(GetPurchaseUsageEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetPurchaseUsageRoute)]
        public async Task<GetPurchaseUsageEntry> GetAsync(Guid purchaseUsageId)
        {
            return await _purchaseUsageService.GetPurchaseUsageAsync(Token, purchaseUsageId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Добавление использования участником продукта
        /// </summary>
        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(PurchaseUsageShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddPurchaseUsageRoute)]
        public async Task<PurchaseUsageShortEntry> PostAsync([FromBody] UpsertPurchaseUsageEntry addEntry)
        {
            return await _purchaseUsageService.AddPurchaseUsageAsync(Token, addEntry, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Удаление использования участником продукта
        /// </summary>
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeletePurchaseUsageRoute)]
        public async Task DeleteAsync(Guid purchaseUsageId)
        {
            await _purchaseUsageService.DeletePurchaseUsageAsync(Token, purchaseUsageId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Узменение использования участником продукта (а надо ли??)
        /// </summary>
        [HttpPatch]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(PurchaseUsageShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UpdatePurchaseUsageRoute)]
        public async Task<PurchaseUsageShortEntry> PatchAsync(Guid purchaseUsageId, [FromBody] UpsertPurchaseUsageEntry updateEntry)
        {
            return await _purchaseUsageService.UpdatePurchaseUsageAsync(Token, purchaseUsageId, updateEntry, HttpContext.RequestAborted);
        }
    }
}
