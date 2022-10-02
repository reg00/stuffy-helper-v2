using EnsureThat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Api.Web;
using StuffyHelper.Authorization.Core.Models;
using StuffyHelper.Core.Features.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace StuffyHelper.Api.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly Authorization.Core.Features.IAuthorizationService _authorizationService;

        public AuthorizationController(Authorization.Core.Features.IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(GetUserEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.RegisterRoute)]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            if (!ModelState.IsValid)
            {
                return Unauthorized(new ErrorResponse(ModelState));
            }

            var user = await _authorizationService.Register(model);

            return Ok(new GetUserEntry(user));
        }

        [HttpPost]
        [ProducesResponseType(typeof(GetUserEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.LoginRoute)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            if (!ModelState.IsValid)
            {
                return Unauthorized(new ErrorResponse(ModelState));
            }

            var token = await _authorizationService.Login(model);

            Response.Headers.Add("token", new JwtSecurityTokenHandler().WriteToken(token));
            Response.Headers.Add("expiration", token.ValidTo.ToString());

            var user = await _authorizationService.GetUser(model.Username);

            return Ok(new GetUserEntry(user));
        }

        [HttpPost]
        [Authorize]
        [Route(KnownRoutes.LogoutRoute)]
        public async Task<IActionResult> Logout()
        {
            await _authorizationService.Logout(HttpContext);

            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IdentityRole), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Authorize(Roles = $"{nameof(UserType.Admin)}")]
        [Route(KnownRoutes.RolesRoute)]
        public IActionResult GetRoles()
        {
            return Ok(_authorizationService.GetRoles());
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.IsAdminRoute)]
        public IActionResult CheckUserIsAdmin()
        {
            return Ok(_authorizationService.CheckUserIsAdmin(User));
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(GetUserEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AccountRoute)]
        public async Task<IActionResult> GetUserByToken()
        {
            var user = await _authorizationService.GetUserByToken(User);

            return Ok(new GetUserEntry(user));
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(UserType.Admin)}")]
        [ProducesResponseType(typeof(IEnumerable<UserEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UserLoginsRoute)]
        public IActionResult GetUserLogins(string userName = null)
        {
            return Ok(_authorizationService.GetUserLogins(userName));
        }
    }
}
