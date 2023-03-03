using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Api.Web;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Debt;
using System.Net;
using IAuthorizationService = StuffyHelper.Authorization.Core.Features.IAuthorizationService;

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
        public async Task<IActionResult> GetDebtsAsync()
        {
            var user = await _authorizationService.GetUserByToken(User, HttpContext.RequestAborted);

            var debtsResponce = await _debtService.GetDebtsByUserAsync(user.Id, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, debtsResponce);
        }
    }
}
