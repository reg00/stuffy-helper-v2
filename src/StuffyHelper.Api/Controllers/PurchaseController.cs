using Microsoft.AspNetCore.Mvc;
using System.Net;
using StuffyHelper.Common.Exceptions;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;
using StuffyHelper.Contracts.Models;
using StuffyHelper.Core.Services.Interfaces;

namespace StuffyHelper.Api.Controllers
{
    /// <summary>
    /// Покупки
    /// </summary>
    public class PurchaseController : AuthorizedApiController
    {
        private readonly IPurchaseService _purchaseService;

        /// <summary>
        /// Ctor.
        /// </summary>
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
            return await _purchaseService.GetPurchasesAsync(offset, limit, name, costMin, costMax, eventId,
                                                                            purchaseTags, unitTypeId, isComplete, HttpContext.RequestAborted);
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
        public async Task<GetPurchaseEntry> GetAsync(Guid purchaseId)
        {
             return await _purchaseService.GetPurchaseAsync(purchaseId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Добавление покупки
        /// </summary>
        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(PurchaseShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddPurchaseRoute)]
        public async Task<PurchaseShortEntry> PostAsync([FromBody] AddPurchaseEntry addEntry)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException(ModelState);
            }

            return await _purchaseService.AddPurchaseAsync(addEntry, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Удаление покупки
        /// </summary>
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeletePurchaseRoute)]
        public async Task DeleteAsync(Guid purchaseId)
        {
            await _purchaseService.DeletePurchaseAsync(purchaseId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Изменение данных о покупке
        /// </summary>
        [HttpPatch]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(PurchaseShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UpdatePurchaseRoute)]
        public async Task<PurchaseShortEntry> PatchAsync(Guid purchaseId, [FromBody] UpdatePurchaseEntry updateEntry)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException(ModelState);
            }

            return await _purchaseService.UpdatePurchaseAsync(purchaseId, updateEntry, HttpContext.RequestAborted);
        }
    }
}
