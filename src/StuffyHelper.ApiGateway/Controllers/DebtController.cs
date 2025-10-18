using Microsoft.AspNetCore.Mvc;
using System.Net;
using StuffyHelper.ApiGateway.Core.Services.Interfaces;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Controllers
{
    public class DebtController : AuthorizedApiController
    {
        private readonly IDebtService _debtService;

        public DebtController(IDebtService debtService)
        {
            _debtService = debtService;
        }

        /// <summary>
        /// Получение списка долгов
        /// </summary>
        /// <param name="eventId">Id ивента (события)</param>
        /// <param name="offset">Кол-во элементов которые необходимо пропустить</param>
        /// <param name="limit">Кол-во элементов на 1 странице</param>
        [HttpGet]
        [ProducesResponseType(typeof(Response<GetDebtEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetDebtsRoute)]
        public async Task<Response<GetDebtEntry>> GetDebtsAsync([FromRoute] Guid eventId, int offset = 0, int limit = 10)
        {
            var debtsResponse = await _debtService.GetDebtsByUserAsync(Token, eventId, offset, limit, HttpContext.RequestAborted);

            return debtsResponse;
        }

        /// <summary>
        /// Получение информации о долге
        /// </summary>
        /// <param name="debtId"></param>
        /// <param name="eventId">Id ивента (события)</param>
        [HttpGet]
        [ProducesResponseType(typeof(GetDebtEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetDebtRoute)]
        public async Task<GetDebtEntry> GetDebtAsync([FromRoute] Guid eventId, [FromRoute] Guid debtId)
        {
            var debt = await _debtService.GetDebtAsync(Token, eventId, debtId, HttpContext.RequestAborted);

            return debt;
        }

        /// <summary>
        /// Оплатить долг
        /// </summary>
        /// <param name="debtId">Id пользователя - должника</param>
        /// <param name="eventId">Id ивента (события)</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]
        [Route(KnownRoutes.SendDebtRoute)]
        public async Task SendDebtAsync([FromRoute] Guid eventId, [FromRoute] Guid debtId)
        {
            await _debtService.SendDebtAsync(Token, eventId, debtId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Оплатить долг
        /// </summary>
        /// <param name="debtId">Id пользователя - должника</param>
        /// <param name="eventId">Id ивента (события)</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]
        [Route(KnownRoutes.ConfirmDebtRoute)]
        public async Task ConfirmDebtAsync([FromRoute] Guid eventId, [FromRoute] Guid debtId)
        {
            await _debtService.ConfirmDebtAsync(Token, eventId, debtId, HttpContext.RequestAborted);
        }
    }
}
