using EnsureThat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using StuffyHelper.Api.Web;
using StuffyHelper.Authorization.Core.Models;
using StuffyHelper.Authorization.Core.Models.User;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.EmailService.Core.Service;
using StuffyHelper.Core.Configs;
using Microsoft.Extensions.Options;

namespace StuffyHelper.Api.Controllers
{
    [Authorize]
    public class AuthorizationController : Controller
    {
        private readonly Authorization.Core.Features.IAuthorizationService _authorizationService;
        private readonly IEmailService _emailService;
        private readonly FrontEndConfiguration _frontEndConfiguration;

        public AuthorizationController(
            Authorization.Core.Features.IAuthorizationService authorizationService,
            IEmailService emailService,
            IOptions<FrontEndConfiguration> options)
        {
            _authorizationService = authorizationService;
            _emailService = emailService;
            _frontEndConfiguration = options.Value;
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.RegisterRoute)]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse(ModelState));
            }

            var code = await _authorizationService.Register(model);

            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Authorization",
                new { login = model.Username, code = code },
                protocol: HttpContext.Request.Scheme);

            await _emailService.SendEmailAsync(model.Email, "Confirm your account",
                $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");

            return Ok("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetUserEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.EmailConfirmRoute)]
        public async Task<IActionResult> ConfirmEmail(string login, string code)
        {
            var user = await _authorizationService.ConfirmEmail(login, code);

            return Redirect($"{_frontEndConfiguration.Endpoint.OriginalString}/register/success");
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
        /// Отправка на email ссылки на сброс пароля
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route(KnownRoutes.ResetPasswordRoute)]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse(ModelState));
            }

            var (name, code) = await _authorizationService.ForgotPasswordAsync(model);

            var callbackUrl = Url.Action(
                "ResetPassword",
                "Authorization",
                new { login = name, code = code },
                protocol: HttpContext.Request.Scheme);

            await _emailService.SendEmailAsync(model.Email, "Reset password",
                $"Для того, чтобы изменить пароль, перейдите по ссылке: {callbackUrl}. Если вы не присылали запрос на изменение пароля, проигнорируйте сообщение.");

            return Ok("Инструкция по изменению пароля отправлена на почту.");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route(KnownRoutes.ResetPasswordConfirmRoute)]
        public IActionResult ResetPassword(string? code = null)
        {
            EnsureArg.IsNotNullOrWhiteSpace(code, nameof(code));

            return Ok(code);
        }

        /// <summary>
        /// Сброс пароля. Выполняется после того, как перейдешь по ссылке на мыле.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route(KnownRoutes.ResetPasswordConfirmRoute)]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse(ModelState));
            }

            await _authorizationService.ResetPasswordAsync(model);

            return Ok("Пароль успешно изменен");
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
            var user = await _authorizationService.GetUserByToken(User);

            return Ok(new GetUserEntry(user));
        }

        /// <summary>
        /// Список зарегистрированных пользователей
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UserLoginsRoute)]
        public async Task<IActionResult> GetUserLogins(string? userName = null)
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
