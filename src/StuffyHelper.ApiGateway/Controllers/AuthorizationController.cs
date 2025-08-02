using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;
using IAuthorizationService = StuffyHelper.ApiGateway.Core.Services.Interfaces.IAuthorizationService;

namespace StuffyHelper.ApiGateway.Controllers
{
    [Authorize]
    public class AuthorizationController : AuthorizedApiController
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
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
        public async Task<string> Register([FromBody] RegisterModel model)
        {
            return await _authorizationService.Register(model, HttpContext.RequestAborted);
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
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.LoginRoute)]
        public async Task<string> Login([FromBody] LoginModel model)
        {
            return await _authorizationService.Login(model, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Отправка на email ссылки на сброс пароля
        /// </summary>
        /// <param name="model">Модель для сброса пароля</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route(KnownRoutes.ResetPasswordRoute)]
        public async Task<string> ForgotPassword(ForgotPasswordModel model)
        {
            return await _authorizationService.ForgotPasswordAsync(model, HttpContext.RequestAborted);
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
        public async Task<ResetPasswordResult> ResetPassword(string email, string code)
        {
            return await _authorizationService.ResetPasswordAsync(email, code, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Сброс пароля. Выполняется после того, как перейдешь по ссылке на мыле.
        /// </summary>
        /// <param name="model">Модель для сброса пароля</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route(KnownRoutes.ResetPasswordConfirmRoute)]
        public async Task<string> ResetPassword(ResetPasswordModel model)
        {
            return await _authorizationService.ConfirmResetPasswordAsync(model, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Список ролей
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IdentityRole), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Forbidden)]
        [Route(KnownRoutes.RolesRoute)]
        public async Task<IReadOnlyList<IdentityRole>> GetRoles()
        {
            return await _authorizationService.GetRoles(Token, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Проверка пользователя на администратора
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.IsAdminRoute)]
        public async Task<bool> CheckUserIsAdmin()
        {
            return await _authorizationService.CheckUserIsAdmin(Token, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Получение данных о пользователе
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(GetUserEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AccountRoute)]
        public async Task<GetUserEntry> GetAccountInfoAsync()
        {
            return await _authorizationService.GetAccountInfoAsync(Token, HttpContext.RequestAborted);
        }
        
        /// <summary>
        /// Получение данных о пользователе по айдишнику
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetUserEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetUserByIdRoute)]
        public async Task<GetUserEntry> GetUserById(string userId)
        {
            return await _authorizationService.GetUserById(userId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Список зарегистрированных пользователей
        /// </summary>
        /// <param name="userName">Логин пользователя. Опциональное поле.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<UserShortEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UserLoginsRoute)]
        public async Task<IReadOnlyList<UserShortEntry>> GetUserLogins(string? userName = null)
        {
            return await _authorizationService.GetUserLogins(Token, userName, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Изменение данных пользователя
        /// </summary>
        /// <param name="updateModel">Модель для обновления информации пользователя</param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(GetUserEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.EditUserRoute)]
        public async Task<GetUserEntry> EditUserAsync([FromBody] UpdateModel updateModel)
        {
            return await _authorizationService.UpdateUserAsync(Token, updateModel, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Изменение аватара пользователя
        /// </summary>
        /// <param name="file">Новый аватар</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AvatarRoute)]
        public async Task EditAvatarAsync(IFormFile file)
        {
            await _authorizationService.UpdateAvatar(Token, file, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Удаление аватара пользователя
        /// </summary>
        [HttpDelete]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AvatarRoute)]
        public async Task RemoveAvatarAsync()
        {
            await _authorizationService.RemoveAvatar(Token, HttpContext.RequestAborted);
        }
    }
}
