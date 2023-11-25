using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Api.Web;
using StuffyHelper.Authorization.Core.Features.Friend;
using StuffyHelper.Core.Features.Common;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace StuffyHelper.Api.Controllers
{
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
        public async Task<IActionResult> GetSendedRequestsAsync()
        {
            var requests = await _requestService.GetSendedRequestsAsync(User, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, requests);
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
        public async Task<IActionResult> GetIncomingRequestsAsync()
        {
            var requests = await _requestService.GetIncomingRequestsAsync(User, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, requests);
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
        public async Task<IActionResult> GetAsync(Guid requestId)
        {
            var request = await _requestService.GetRequestAsync(requestId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, request);
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
        public async Task<IActionResult> AcceptAsync(Guid requestId)
        {
            await _requestService.AcceptRequest(requestId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK);
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
        public async Task<IActionResult> PostAsync([Required] string userId)
        {
            var request = await _requestService.AddRequestAsync(User, userId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, request);
        }

        /// <summary>
        /// Удалить заявку в друзья
        /// </summary>
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeleteRequestRoute)]
        public async Task<IActionResult> DeleteAsync(Guid requestId)
        {
            await _requestService.DeleteRequestAsync(requestId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK);
        }
    }
}
