using EnsureThat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Api.Web;
using StuffyHelper.Authorization.Core.Models;
using StuffyHelper.Core.Features.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using StuffyHelper.Authorization.Core.Models.User;

namespace StuffyHelper.Api.Controllers
{
    [Authorize]
    public class AuthorizationController : Controller
    {
        private readonly Authorization.Core.Features.IAuthorizationService _authorizationService;

        public AuthorizationController(Authorization.Core.Features.IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetUserEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.RegisterRoute)]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse(ModelState));
            }

            var user = await _authorizationService.Register(model);

            return Ok(new GetUserEntry(user));
        }

        /// <summary>
        /// Логин
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetUserEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.LoginRoute)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse(ModelState));
            }

            var token = await _authorizationService.Login(model, HttpContext);
            
            Response.Headers.Add("token", new JwtSecurityTokenHandler().WriteToken(token));
            Response.Headers.Add("expiration", token.ValidTo.ToString());

            var user = await _authorizationService.GetUser(model.Username);

            return Ok(new GetUserEntry(user));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/auth/signin-google")]
        public IActionResult GoogleLogin()
        {
            string redirectUrl = Url.Action("GoogleResponse", "Authorization");
            var properties = _authorizationService.GoogleLogin(redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/auth/signin-google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            await _authorizationService.GoogleResponse();
            return Redirect("account");
        }

        /// <summary>
        /// Выход
        /// </summary>
        [HttpPost]
        [Route(KnownRoutes.LogoutRoute)]
        public async Task<IActionResult> Logout()
        {
            await _authorizationService.Logout(HttpContext);
            return Ok();
        }

        /// <summary>
        /// Список ролей
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IdentityRole), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.RolesRoute)]
        public async Task<IActionResult> GetRoles()
        {
            var isAdmin = await _authorizationService.CheckUserIsAdmin(User, HttpContext.RequestAborted);

            return isAdmin ? Ok(_authorizationService.GetRoles()) : Unauthorized();
        }

        /// <summary>
        /// Проверка пользователя на администратора
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.IsAdminRoute)]
        public async Task<IActionResult> CheckUserIsAdmin()
        {
            var isAdmin = await _authorizationService.CheckUserIsAdmin(User, HttpContext.RequestAborted);
            return Ok(isAdmin);
        }

        /// <summary>
        /// Получение данных о пользователе
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(GetUserEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AccountRoute)]
        public async Task<IActionResult> GetAccountInfoAsync()
        {
            var user = await _authorizationService.GetAccountInfoAsync(User, HttpContext.RequestAborted);
            return Ok(new GetUserEntry(user));
        }

        /// <summary>
        /// Список зарегистрированных пользователей
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UserLoginsRoute)]
        public async Task<IActionResult> GetUserLogins(string userName = null)
        {
            var isAdmin = await _authorizationService.CheckUserIsAdmin(User, HttpContext.RequestAborted);

            return isAdmin ? Ok(_authorizationService.GetUserLogins(userName)) : Unauthorized();
        }

        /// <summary>
        /// Изменение данных пользователя
        /// </summary>
        [HttpPatch]
        [ProducesResponseType(typeof(UserEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.EditUserRoute)]
        public async Task<IActionResult> EditUserAsync(UpdateModel updateModel)
        {
            EnsureArg.IsNotNull(updateModel, nameof(updateModel));

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse(ModelState));
            }

            var user = await _authorizationService.UpdateUser(User, updateModel);

            return Ok(new GetUserEntry(user));
        }
    }
}
