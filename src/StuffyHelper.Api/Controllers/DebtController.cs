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
    /// Долги
    /// </summary>
    public class DebtController : AuthorizedApiController
    {
        private readonly IDebtService _debtService;
        private StuffyClaims UserClams => IdentityClaims.GetUserClaims();
        
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
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetDebtsRoute)]
        public async Task<Response<GetDebtEntry>> GetDebtsAsync(int offset = 0, int limit = 10)
        {
            return await _debtService.GetDebtsByUserAsync(UserClams.UserId, offset, limit, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Получение информации о долге
        /// </summary>
        /// <param name="debtId"></param>
        [HttpGet]
        [ProducesResponseType(typeof(GetDebtEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetDebtRoute)]
        public async Task<GetDebtEntry> GetDebtAsync([FromRoute] Guid debtId)
        {
            return await _debtService.GetDebtAsync(debtId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Оплатить долг
        /// </summary>
        /// <param name="debtId">Id пользователя - должника</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]
        [Route(KnownRoutes.SendDebtRoute)]
        public async Task SendDebtAsync([FromRoute] Guid debtId)
        {
            await _debtService.SendDebtAsync(UserClams.UserId, debtId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Оплатить долг
        /// </summary>
        /// <param name="debtId">Id пользователя - должника</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]
        [Route(KnownRoutes.ConfirmDebtRoute)]
        public async Task ConfirmDebtAsync([FromRoute] Guid debtId)
        {
            await _debtService.ConfirmDebtAsync(UserClams.UserId, debtId, HttpContext.RequestAborted);
        }
    }
}
