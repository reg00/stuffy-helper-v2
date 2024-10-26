using System.IdentityModel.Tokens.Jwt;
using EnsureThat;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using StuffyHelper.Authorization.Api.Web;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Configurations;
using StuffyHelper.Common.Messages;
using StuffyHelper.EmailService.Contracts.Clients.Interfaces;
using StuffyHelper.EmailService.Contracts.Models;
using IAuthorizationService = StuffyHelper.Authorization.Core.Services.Interfaces.IAuthorizationService;

namespace StuffyHelper.Authorization.Api.Controllers
{
    [Authorize]
    public class AuthorizationController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IStuffyEmailClient _emailClient;
        private readonly FrontEndConfiguration _frontEndConfiguration;

        public AuthorizationController(
            IAuthorizationService authorizationService,
            IStuffyEmailClient emailClient,
            IOptions<FrontEndConfiguration> options)
        {
            _authorizationService = authorizationService;
            _emailClient = emailClient;
            _frontEndConfiguration = options.Value;
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="model">Модель для регистрации</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.RegisterRoute)]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse(ModelState));
            }

            var code = await _authorizationService.Register(model);
            try
            {
                var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Authorization",
                new { login = model.Username, code },
                protocol: HttpContext.Request.Scheme,
                _frontEndConfiguration.Endpoint.OriginalString);

                var request = new SendEmailRequest()
                {
                    Email = model.Email,
                    Subject = "Confirm your account",
                    Message = $"Подтвердите регистрацию, перейдя по <a href='{callbackUrl}'>ссылке</a>."
                };
                
                await _emailClient.SendAsync(request);

            }
            catch (Exception)
            {
                await _authorizationService.DeleteUser(model.Username);

                throw;
            }

            return Ok("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");
        }

        /// <summary>
        /// Подтверждение почты
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="code">Код, отправленный на почту</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.EmailConfirmRoute)]
        public async Task<IActionResult> ConfirmEmail(string login, string code)
        {
            await _authorizationService.ConfirmEmail(login, code);

            return Redirect("~/sign-up/success");
        }

        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="model">Модель для логина</param>
        /// <returns></returns>
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

            var user = await _authorizationService.GetUserByName(model.Username);

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
        /// <param name="model">Модель для сброса пароля</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route(KnownRoutes.ResetPasswordRoute)]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse(ModelState));
            }

            var (_, code) = await _authorizationService.ForgotPasswordAsync(model);

            var callbackUrl = Url.Action(
                "ResetPassword",
                "Authorization",
                new { email = model.Email, code },
                protocol: HttpContext.Request.Scheme,
                _frontEndConfiguration.Endpoint.OriginalString);

            var request = new SendEmailRequest()
            {
                Email = model.Email,
                Subject = "Reset password",
                Message = $"Для того, чтобы изменить пароль, перейдите по этой: <a href='{callbackUrl}'>ссылке</a>. Если вы не присылали запрос на изменение пароля, проигнорируйте сообщение."
            };

            await _emailClient.SendAsync(request);

            return Ok("Инструкция по изменению пароля отправлена на почту.");
        }

        /// <summary>
        /// Сброс пароля. Вызывается при клике на ссылку из почты.
        /// </summary>
        /// <param name="email">Почта пользователя</param>
        /// <param name="code">Код из письма</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route(KnownRoutes.ResetPasswordConfirmRoute)]
        public IActionResult ResetPassword(string email, string code)
        {
            EnsureArg.IsNotNullOrWhiteSpace(code, nameof(code));
            EnsureArg.IsNotNullOrWhiteSpace(email, nameof(email));

            return Redirect($"~/password-reset/confirm?email={email}&code={code}");
        }

        /// <summary>
        /// Сброс пароля. Выполняется после того, как перейдешь по ссылке на мыле.
        /// </summary>
        /// <param name="model">Модель для сброса пароля</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route(KnownRoutes.ResetPasswordConfirmRoute)]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));

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
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Forbidden)]
        [Route(KnownRoutes.RolesRoute)]
        public async Task<IActionResult> GetRoles()
        {
            var isAdmin = await _authorizationService.CheckUserIsAdmin(User, HttpContext.RequestAborted);

            return isAdmin ? Ok(_authorizationService.GetRoles()) : Forbid();
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
        /// <param name="userName">Логин пользователя. Опциональное поле.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UserLoginsRoute)]
        public IActionResult GetUserLogins(string? userName = null)
        {
            return Ok(_authorizationService.GetUserLogins(userName));
        }

        /// <summary>
        /// Изменение данных пользователя
        /// </summary>
        /// <param name="updateModel">Модель для обновления информации пользователя</param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(UserEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.EditUserRoute)]
        public async Task<IActionResult> EditUserAsync([FromBody] UpdateModel updateModel)
        {
            EnsureArg.IsNotNull(updateModel, nameof(updateModel));

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse(ModelState));
            }

            var user = await _authorizationService.UpdateUser(User, updateModel);

            return Ok(new GetUserEntry(user));
        }

        /// <summary>
        /// Изменение аватара пользователя
        /// </summary>
        /// <param name="file">Новый аватар</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AvatarRoute)]
        public async Task<IActionResult> EditAvatarAsync(IFormFile file)
        {
            EnsureArg.IsNotNull(file, nameof(file));

            await _authorizationService.UpdateAvatar(User, file);

            return Ok();
        }

        /// <summary>
        /// Удаление аватара пользователя
        /// </summary>
        [HttpDelete]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AvatarRoute)]
        public async Task<IActionResult> RemoveAvatarAsync()
        {
            await _authorizationService.RemoveAvatar(User);

            return Ok();
        }
    }
}
