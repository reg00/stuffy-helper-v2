using Microsoft.AspNetCore.Mvc;
using System.Net;
using StuffyHelper.ApiGateway.Core.Services.Interfaces;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Controllers
{
    public class PurchaseController : AuthorizedApiController
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
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetPurchasesRoute)]
        public async Task<Response<GetPurchaseEntry>> GetAsync(
            Guid eventId,
            int offset = 0,
            int limit = 10,
            string? name = null,
            long? costMin = null,
            long? costMax = null,
            bool? isComplete = null,
            Guid[]? purchaseIds = null)
        {
            return await _purchaseService.GetPurchasesAsync(Token, eventId, offset, limit, name, costMin, costMax,
                                                                            isComplete, purchaseIds, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Получение данных о покупке по идентификатору
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(GetPurchaseEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetPurchaseRoute)]
        public async Task<GetPurchaseEntry> GetAsync([FromRoute] Guid eventId, [FromRoute] Guid purchaseId)
        {
            return await _purchaseService.GetPurchaseAsync(Token, eventId, purchaseId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Добавление покупки
        /// </summary>
        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(PurchaseShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddPurchaseRoute)]
        public async Task<PurchaseShortEntry> PostAsync([FromRoute] Guid eventId, [FromBody] AddPurchaseEntry addEntry)
        {
            return await _purchaseService.AddPurchaseAsync(Token, eventId, addEntry, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Удаление покупки
        /// </summary>
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeletePurchaseRoute)]
        public async Task DeleteAsync([FromRoute] Guid eventId, [FromRoute] Guid purchaseId)
        {
            await _purchaseService.DeletePurchaseAsync(Token, eventId, purchaseId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Изменение данных о покупке
        /// </summary>
        [HttpPatch]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(PurchaseShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UpdatePurchaseRoute)]
        public async Task<PurchaseShortEntry> PatchAsync([FromRoute] Guid eventId, [FromRoute] Guid purchaseId, [FromBody] UpdatePurchaseEntry updateEntry)
        {
            return await _purchaseService.UpdatePurchaseAsync(Token, eventId, purchaseId, updateEntry, HttpContext.RequestAborted);
        }
    }
}
