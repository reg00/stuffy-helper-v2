using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Api.Web;
using StuffyHelper.Authorization.Core.Features.FriendsRequest;
using StuffyHelper.Core.Features.Common;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace StuffyHelper.Api.Controllers
{
    public class FriendsRequestController : Controller
    {
        private readonly IFriendsRequestService _requestService;

        public FriendsRequestController(IFriendsRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FriendsRequestShort>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetSendedRequestsRoute)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetSendedRequestsAsync()
        {
            var requests = await _requestService.GetSendedRequestsAsync(User, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, requests);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FriendsRequestShort>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetIncomingRequestsRoute)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetIncomingRequestsAsync()
        {
            var requests = await _requestService.GetIncomingRequestsAsync(User, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, requests);
        }

        [HttpGet]
        [ProducesResponseType(typeof(FriendsRequestShort), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetRequestRoute)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAsync(Guid requestId)
        {
            var request = await _requestService.GetRequestAsync(requestId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, request);
        }

        [HttpPost]
        [Consumes(KnownContentTypes.MultipartFormData)]
        [RequestSizeLimit(int.MaxValue)]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        [ProducesResponseType(typeof(FriendsRequestShort), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddRequestRoute)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostAsync([Required]string userId)
        {
            var request = await _requestService.AddRequestAsync(User, userId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, request);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeleteRequestRoute)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteAsync(Guid requestId)
        {
            await _requestService.DeleteRequestAsync(requestId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK);
        }
    }
}
