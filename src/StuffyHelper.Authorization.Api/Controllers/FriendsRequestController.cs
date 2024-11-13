using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Authorization.Core.Services.Interfaces;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;

namespace StuffyHelper.Authorization.Api.Controllers;

[Authorize]
public class FriendsRequestController : Controller
{
     private readonly IFriendsRequestService _requestService;

        public FriendsRequestController(IFriendsRequestService requestService)
        {
            _requestService = requestService;
        }

        /// <summary>
        /// Получение списка отправленных заявок в друзья
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FriendsRequestShort>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetSendedRequestsRoute)]
        public async Task<IReadOnlyList<FriendsRequestShort>> GetSendedRequestsAsync()
        {
            var requests = await _requestService.GetSendedRequestsAsync(User, HttpContext.RequestAborted);

            return requests;
        }

        /// <summary>
        /// Получение списка входящих заявок в друзья
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FriendsRequestShort>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetIncomingRequestsRoute)]
        public async Task<IReadOnlyList<FriendsRequestShort>> GetIncomingRequestsAsync()
        {
            var requests = await _requestService.GetIncomingRequestsAsync(User, HttpContext.RequestAborted);

            return requests;
        }

        /// <summary>
        /// Получение информации о заявке в друзья по идентификатору
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(FriendsRequestShort), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetRequestRoute)]
        public async Task<FriendsRequestShort> GetAsync(Guid requestId)
        {
            var request = await _requestService.GetRequestAsync(requestId, HttpContext.RequestAborted);

            return request;
        }

        /// <summary>
        /// Принять заявку в друзья
        /// </summary>
        [HttpPost]
        [Consumes(KnownContentTypes.MultipartFormData)]
        [RequestSizeLimit(int.MaxValue)]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AcceptRequestRoute)]
        public async Task ConfirmAsync(Guid requestId)
        {
            await _requestService.ConfirmRequest(requestId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Отправить заявку в друзья
        /// </summary>
        [HttpPost]
        [Consumes(KnownContentTypes.MultipartFormData)]
        [RequestSizeLimit(int.MaxValue)]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        [ProducesResponseType(typeof(FriendsRequestShort), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddRequestRoute)]
        public async Task<FriendsRequestShort> PostAsync([Required] string userId)
        {
            var request = await _requestService.AddRequestAsync(User, userId, HttpContext.RequestAborted);

            return request;
        }

        /// <summary>
        /// Удалить заявку в друзья
        /// </summary>
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeleteRequestRoute)]
        public async Task DeleteAsync(Guid requestId)
        {
            await _requestService.DeleteRequestAsync(requestId, HttpContext.RequestAborted);
        }
}