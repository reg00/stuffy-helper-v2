using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Api.Web;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Debt;
using System.Net;
using IAuthorizationService = StuffyHelper.Authorization.Core1.Features.IAuthorizationService;

namespace StuffyHelper.Api.Controllers
{
    [Authorize]
    public class DebtController : Controller
    {
        private readonly IDebtService _debtService;
        private readonly IAuthorizationService _authorizationService;

        public DebtController(
            IDebtService debtService,
            IAuthorizationService authorizationService)
        {
            _debtService = debtService;
            _authorizationService = authorizationService;
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
        public async Task<IActionResult> GetDebtsAsync(int offset = 0, int limit = 10)
        {
            var user = await _authorizationService.GetUserByToken(User, HttpContext.RequestAborted);

            var debtsResponce = await _debtService.GetDebtsByUserAsync(user.Id, offset, limit, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, debtsResponce);
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
        public async Task<IActionResult> SendDebtAsync([FromRoute] Guid debtId)
        {
            var user = await _authorizationService.GetUserByToken(User, HttpContext.RequestAborted);

            await _debtService.SendDebtAsync(user.Id, debtId, HttpContext.RequestAborted);

            return Ok("Succeccfully send debt");
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
        public async Task<IActionResult> ConfirmDebtAsync([FromRoute] Guid debtId)
        {
            var user = await _authorizationService.GetUserByToken(User, HttpContext.RequestAborted);

            await _debtService.ConfirmDebtAsync(user.Id, debtId, HttpContext.RequestAborted);

            return Ok("Succeccfully confirm debt");
        }
    }
}
