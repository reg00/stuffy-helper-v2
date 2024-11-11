using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Api.Web;
using StuffyHelper.Core.Features.Debt;
using System.Net;
using StuffyHelper.Common.Contracts;
using StuffyHelper.Common.Messages;

namespace StuffyHelper.Api.Controllers
{
    [Authorize]
    public class DebtController : Controller
    {
        private readonly IDebtService _debtService;
        private readonly IAuthorizationService _authorizationService;

        private StuffyClaims StuffyClaims => new StuffyClaims();
        
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
        [ProducesResponseType(typeof(Core.Features.Common.Response<GetDebtEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetDebtsRoute)]
        public async Task<IActionResult> GetDebtsAsync(int offset = 0, int limit = 10)
        {
            var debtsResponce = await _debtService.GetDebtsByUserAsync(StuffyClaims.UserId, offset, limit, HttpContext.RequestAborted);

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
            await _debtService.SendDebtAsync(StuffyClaims.UserId, debtId, HttpContext.RequestAborted);

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
            await _debtService.ConfirmDebtAsync(StuffyClaims.UserId, debtId, HttpContext.RequestAborted);

            return Ok("Succeccfully confirm debt");
        }
    }
}
