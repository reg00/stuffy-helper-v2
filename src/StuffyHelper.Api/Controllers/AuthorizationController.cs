using EnsureThat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Api.Web;
using StuffyHelper.Authorization.Core.Models;
using System.IdentityModel.Tokens.Jwt;

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
        [Route(KnownRoutes.LoginRoute)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            if (!ModelState.IsValid)
            {
                var error = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return Unauthorized(new Response() { Status = "Error", Message = error });
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
        [Authorize(Roles = $"{nameof(UserType.Admin)}")]
        [Route(KnownRoutes.RolesRoute)]
        public IActionResult GetRoles()
        {
            return Ok(_authorizationService.GetRoles());
        }

        [HttpGet]
        [Authorize]
        [Route(KnownRoutes.IsAdminRoute)]
        public IActionResult CheckUserIsAdmin()
        {
            return Ok(_authorizationService.CheckUserIsAdmin(User));
        }

        [HttpGet]
        [Authorize]
        [Route(KnownRoutes.AccountRoute)]
        public async Task<IActionResult> GetUserByToken()
        {
            var user = await _authorizationService.GetUserByToken(User);

            return Ok(new GetUserEntry(user));
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(UserType.Admin)}")]
        [Route(KnownRoutes.UserLoginsRoute)]
        public IActionResult GetUserLogins(string userName = null)
        {
            return Ok(_authorizationService.GetUserLogins(userName));
        }
    }
}
