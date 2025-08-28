using System.IdentityModel.Tokens.Jwt;
using EnsureThat;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Configurations;
using StuffyHelper.Common.Configurators;
using StuffyHelper.Common.Exceptions;
using StuffyHelper.Common.Helpers;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;
using StuffyHelper.EmailService.Contracts.Clients.Interfaces;
using StuffyHelper.EmailService.Contracts.Models;
using IAuthorizationService = StuffyHelper.Authorization.Core.Services.Interfaces.IAuthorizationService;

namespace StuffyHelper.Authorization.Api.Controllers
{
    /// <summary>
    /// Контроллер для работы с авторизацией
    /// </summary>
    [Authorize]
    public class AuthorizationController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IStuffyEmailClient _emailClient;
        private readonly FrontEndConfiguration _frontEndConfiguration;
        private readonly AuthorizationConfiguration _authorizationConfiguration;

        /// <summary>
        /// Ctor.
        /// </summary>
        public AuthorizationController(
            IAuthorizationService authorizationService,
            IStuffyEmailClient emailClient,
            IConfiguration configuration)
        {
            _authorizationService = authorizationService;
            _emailClient = emailClient;

            var config = configuration.GetConfig();
            
            _frontEndConfiguration = config.Frontend;
            _authorizationConfiguration = config.Authorization;
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="model">Модель для регистрации</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.RegisterRoute)]
        public async Task<string> Register([FromBody] RegisterModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            if (!ModelState.IsValid)
                throw new BadRequestException(ModelState);

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

                var token = TokenHelper.GenerateSystemToken(_authorizationConfiguration);
                await _emailClient.SendAsync(token, request, HttpContext.RequestAborted);

            }
            catch (Exception)
            {
                await _authorizationService.DeleteUser(model.Username);

                throw;
            }

            return "Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме";
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
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.EmailConfirmRoute)]
        public async Task ConfirmEmail(string login, string code)
        {
            await _authorizationService.ConfirmEmail(login, code);
        }

        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="model">Модель для логина</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetUserEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.LoginRoute)]
        public async Task<string> Login([FromBody] LoginModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            if (!ModelState.IsValid)
                throw new BadRequestException(ModelState);

            var token = await _authorizationService.Login(model, HttpContext);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Отправка на email ссылки на сброс пароля
        /// </summary>
        /// <param name="model">Модель для сброса пароля</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route(KnownRoutes.ResetPasswordRoute)]
        public async Task<string> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            if (!ModelState.IsValid)
                throw new BadRequestException(ModelState);

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

            var token = TokenHelper.GenerateSystemToken(_authorizationConfiguration);
            await _emailClient.SendAsync(token, request);

            return "Инструкция по изменению пароля отправлена на почту.";
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
        public ResetPasswordResult ResetPassword(string email, string code)
        {
            EnsureArg.IsNotNullOrWhiteSpace(code, nameof(code));
            EnsureArg.IsNotNullOrWhiteSpace(email, nameof(email));

            return new ResetPasswordResult()
            {
                Email = email,
                Code = code
            };
        }

        /// <summary>
        /// Сброс пароля. Выполняется после того, как перейдешь по ссылке на мыле.
        /// </summary>
        /// <param name="model">Модель для сброса пароля</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route(KnownRoutes.ResetPasswordConfirmRoute)]
        public async Task<string> ResetPassword([FromBody] ResetPasswordModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            if (!ModelState.IsValid)
                throw new BadRequestException(ModelState);

            await _authorizationService.ResetPasswordAsync(model);

            return "Пароль успешно изменен";
        }

        /// <summary>
        /// Список ролей
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IdentityRole), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.Forbidden)]
        [Route(KnownRoutes.RolesRoute)]
        public async Task<IReadOnlyList<IdentityRole>> GetRoles()
        {
            var isAdmin = await _authorizationService.CheckUserIsAdmin(User, HttpContext.RequestAborted);

            if (!isAdmin)
                throw new ForbiddenException("You dont have permissions");
            
            return _authorizationService.GetRoles();
        }

        /// <summary>
        /// Проверка пользователя на администратора
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.IsAdminRoute)]
        public async Task<bool> CheckUserIsAdmin()
        {
            return await _authorizationService.CheckUserIsAdmin(User, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Получение данных о пользователе
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(GetUserEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AccountRoute)]
        public async Task<GetUserEntry> GetAccountInfoAsync()
        {
            return await _authorizationService.GetUserByToken(User);
        }
        
        /// <summary>
        /// Получение данных о пользователе по айдишнику
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetUserEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetUserByIdRoute)]
        public async Task<GetUserEntry> GetUserById(string userId)
        {
            return await _authorizationService.GetUserById(userId);
        }

        /// <summary>
        /// Список зарегистрированных пользователей
        /// </summary>
        /// <param name="userName">Логин пользователя. Опциональное поле.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<UserShortEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UserLoginsRoute)]
        public IReadOnlyList<UserShortEntry> GetUserLogins(string? userName = null)
        {
            return _authorizationService.GetUserLogins(userName);
        }

        /// <summary>
        /// Изменение данных пользователя
        /// </summary>
        /// <param name="updateModel">Модель для обновления информации пользователя</param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(GetUserEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.EditUserRoute)]
        public async Task<GetUserEntry> EditUserAsync([FromBody] UpdateModel updateModel)
        {
            EnsureArg.IsNotNull(updateModel, nameof(updateModel));

            if (!ModelState.IsValid)
                throw new BadRequestException(ModelState);

            return await _authorizationService.UpdateUser(User, updateModel);
        }

        /// <summary>
        /// Изменение аватара пользователя
        /// </summary>
        /// <param name="file">Новый аватар</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AvatarRoute)]
        public async Task EditAvatarAsync(IFormFile file)
        {
            EnsureArg.IsNotNull(file, nameof(file));

            await _authorizationService.UpdateAvatar(User, file);
        }

        /// <summary>
        /// Удаление аватара пользователя
        /// </summary>
        [HttpDelete]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AvatarRoute)]
        public async Task RemoveAvatarAsync()
        {
            await _authorizationService.RemoveAvatar(User);
        }
    }
}
