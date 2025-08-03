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
        [HttpGet]
        [ProducesResponseType(typeof(Response<GetDebtEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetDebtsRoute)]
        public async Task<Response<GetDebtEntry>> GetDebtsAsync(int offset = 0, int limit = 10)
        {
            var debtsResponse = await _debtService.GetDebtsByUserAsync(Token, offset, limit, HttpContext.RequestAborted);

            return debtsResponse;
        }

        /// <summary>
        /// Получение информации о долге
        /// </summary>
        /// <param name="debtId"></param>
        [HttpGet]
        [ProducesResponseType(typeof(GetDebtEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetDebtRoute)]
        public async Task<GetDebtEntry?> GetDebtAsync([FromRoute] Guid debtId)
        {
            var debt = await _debtService.GetDebtAsync(Token, debtId, HttpContext.RequestAborted);

            return debt;
        }

        /// <summary>
        /// Оплатить долг
        /// </summary>
        /// <param name="debtId">Id пользователя - должника</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [Route(KnownRoutes.SendDebtRoute)]
        public async Task SendDebtAsync([FromRoute] Guid debtId)
        {
            await _debtService.SendDebtAsync(Token, debtId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Оплатить долг
        /// </summary>
        /// <param name="debtId">Id пользователя - должника</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [Route(KnownRoutes.ConfirmDebtRoute)]
        public async Task ConfirmDebtAsync([FromRoute] Guid debtId)
        {
            await _debtService.ConfirmDebtAsync(Token, debtId, HttpContext.RequestAborted);
        }
    }
}
