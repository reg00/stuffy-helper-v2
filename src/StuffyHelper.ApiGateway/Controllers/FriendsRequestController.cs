using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.ApiGateway.Core.Services.Interfaces;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;

namespace StuffyHelper.ApiGateway.Controllers;

public class FriendsRequestController : AuthorizedApiController
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
        public async Task<IReadOnlyList<FriendsRequestShort>> GetSentRequestsAsync()
        {
            return await _requestService.GetSentRequestsAsync(Token, HttpContext.RequestAborted);
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
            return await _requestService.GetIncomingRequestsAsync(Token, HttpContext.RequestAborted);
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
            return await _requestService.GetRequestAsync(Token, requestId, HttpContext.RequestAborted);
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
            await _requestService.ConfirmRequest(Token, requestId, HttpContext.RequestAborted);
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
            return await _requestService.AddRequestAsync(Token, userId, HttpContext.RequestAborted);
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
            await _requestService.DeleteRequestAsync(Token, requestId, HttpContext.RequestAborted);
        }
}