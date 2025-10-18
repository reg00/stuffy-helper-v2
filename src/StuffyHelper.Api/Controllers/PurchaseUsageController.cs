using Microsoft.AspNetCore.Mvc;
using System.Net;
using StuffyHelper.Common.Contracts;
using StuffyHelper.Common.Helpers;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;
using StuffyHelper.Contracts.Models;
using StuffyHelper.Core.Services.Interfaces;

namespace StuffyHelper.Api.Controllers
{
    /// <summary>
    /// Причастность к покупке
    /// </summary>
    public class PurchaseUsageController : AuthorizedApiController
    {
        private readonly IPurchaseUsageService _purchaseUsageService;

        private StuffyClaims UserClaims => IdentityClaims.GetUserClaims();
        
        /// <summary>
        /// Ctor.
        /// </summary>
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
            Guid eventId,
            int offset = 0,
            int limit = 10,
            Guid? participantId = null,
            Guid? purchaseId = null)
        {
            return await _purchaseUsageService.GetPurchaseUsagesAsync(eventId, offset, limit, participantId, purchaseId, HttpContext.RequestAborted);
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
        public async Task<GetPurchaseUsageEntry> GetAsync([FromRoute] Guid eventId, [FromRoute]Guid purchaseUsageId)
        {
            return await _purchaseUsageService.GetPurchaseUsageAsync(UserClaims, eventId, purchaseUsageId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Добавление использования участником продукта
        /// </summary>
        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(PurchaseUsageShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddPurchaseUsageRoute)]
        public async Task<PurchaseUsageShortEntry> PostAsync([FromRoute] Guid eventId, [FromBody] UpsertPurchaseUsageEntry addEntry)
        {
            return await _purchaseUsageService.AddPurchaseUsageAsync(eventId, addEntry, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Удаление использования участником продукта
        /// </summary>
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeletePurchaseUsageRoute)]
        public async Task DeleteAsync([FromRoute] Guid eventId, [FromRoute] Guid purchaseUsageId)
        {
            await _purchaseUsageService.DeletePurchaseUsageAsync(eventId, purchaseUsageId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Узменение использования участником продукта (а надо ли??)
        /// </summary>
        [HttpPatch]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(PurchaseUsageShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UpdatePurchaseUsageRoute)]
        public async Task<PurchaseUsageShortEntry> PatchAsync([FromRoute] Guid eventId, Guid purchaseUsageId, [FromBody] UpsertPurchaseUsageEntry updateEntry)
        {
            return await _purchaseUsageService.UpdatePurchaseUsageAsync(eventId, purchaseUsageId, updateEntry, HttpContext.RequestAborted);
        }
    }
}
