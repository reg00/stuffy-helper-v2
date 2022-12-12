using EnsureThat;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using StuffyHelper.Authorization.Core.Configs;
using StuffyHelper.Authorization.Core.Exceptions;
using StuffyHelper.Authorization.Core.Extensions;
using StuffyHelper.Authorization.Core.Models;
using StuffyHelper.Authorization.Core.Models.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StuffyHelper.Authorization.Core.Features.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<StuffyUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AuthorizationConfiguration _authorizationConfiguration;

        public AuthorizationService(
            UserManager<StuffyUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<AuthorizationConfiguration> authorizationConfiguration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _authorizationConfiguration = authorizationConfiguration.Value;
        }

        public async Task<JwtSecurityToken> Login(LoginModel model, HttpContext httpContext)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            var user = await _userManager.FindByNameAsync(model.Username);

            if (!await _userManager.IsEmailConfirmedAsync(user))
                throw new AuthorizationException("Вы не подтвердили свой email");

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = await user.CreateToken(_userManager, _authorizationConfiguration);
                var roles = await _userManager.GetRolesAsync(user);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

                foreach (var role in roles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role));
                }

                var principal = new ClaimsPrincipal(identity);
                await httpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        AllowRefresh = true,
                        ExpiresUtc = DateTime.UtcNow.AddHours(_authorizationConfiguration.JWT.TokenExpireInHours)
                    });

                return token;
            }
            else if (user is null)
                throw new AuthorizationResourceNotFoundException($"Пользователь с логином {model.Username} отсутствует");
            else
                throw new AuthorizationException($"Неверный пароль");
        }

        public async Task Logout(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        }

        public async Task<string> Register(RegisterModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            var userExists = await _userManager.FindByNameAsync(model.Username);

            if (userExists != null)
                throw new AuthorizationEntityAlreadyExistsException($"Пользователь с логином {model.Username} уже существует");

            StuffyUser identityUser = model.InitializeUser();

            var result = await _userManager.CreateAsync(identityUser, model.Password);

            if (!result.Succeeded)
                throw new AuthorizationException($"Ошибка создания пользователя! Детали: {string.Join(' ', result.Errors.Select(x => x.Description))}");

            await _roleManager.CreateRolesIfNotExists();
            await identityUser.AddRoleToUser(_roleManager, _userManager, UserType.User);

            return await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
        }

        public IEnumerable<IdentityRole> GetRoles() => _roleManager.Roles.ToList();

        public async Task<bool> CheckUserIsAdmin(ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            var stuffyUser = await _userManager.FindByNameAsync(user?.Identity?.Name);
            return await _userManager.IsInRoleAsync(stuffyUser, nameof(UserType.Admin));
        } 

        public async Task<UserEntry> GetUserByToken(ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            var identityUser = await _userManager.FindByNameAsync(user?.Identity?.Name);
            var rolesList = await _userManager.GetRolesAsync(identityUser);

            return new UserEntry(identityUser, rolesList);
        }

        public IEnumerable<UserEntry> GetUserLogins(string userName = null)
        {
            var users = _userManager.Users
                .Where(u => userName == null || u.UserName.ToLower().StartsWith(userName.ToLower()))
                .Select(u => new UserEntry() { Id = u.Id, Name = u.UserName }).ToList();

            return users;
        }

        public async Task DeleteUser(string userName = null, string userId = null)
        {
            var error = string.Empty;

            if (string.IsNullOrWhiteSpace(userName) && string.IsNullOrWhiteSpace(userId))
                throw new AuthorizationException("UserName or UserId required");

            StuffyUser userToDelete;

            if (!string.IsNullOrWhiteSpace(userName))
                userToDelete = await _userManager.FindByNameAsync(userName);
            else
                userToDelete = await _userManager.FindByIdAsync(userId);

            if (userToDelete is null)
                throw new AuthorizationResourceNotFoundException($"Пользователь с логином {userName} отсутствует");

            var rolesList = await _userManager.GetRolesAsync(userToDelete);

            await _userManager.RemoveFromRolesAsync(userToDelete, rolesList);
            await _userManager.DeleteAsync(userToDelete);
        }

        public async Task<UserEntry> UpdateUser(ClaimsPrincipal user, UpdateModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));
            EnsureArg.IsNotNull(user, nameof(user));

            var userToUpdate = await _userManager.FindByNameAsync(user.Identity.Name);
            if (userToUpdate is null)
                throw new AuthorizationResourceNotFoundException($"Пользователь с логином {user.Identity.Name} отсутствует");

            userToUpdate.PatchFrom(model);

            var updateUserResult = await _userManager.UpdateAsync(userToUpdate);
            var updatedUser = await _userManager.FindByIdAsync(userToUpdate.Id);
            updateUserResult.HandleIdentityResult();
            var rolesList = await _userManager.GetRolesAsync(updatedUser);

            return new UserEntry(updatedUser, rolesList);
        }

        public async Task<UserEntry> GetUser(string userName = null, string userId = null)
        {
            if (string.IsNullOrWhiteSpace(userName) && string.IsNullOrWhiteSpace(userId))
                throw new AuthorizationException("UserName or UserId required");

            StuffyUser user;

            if (!string.IsNullOrWhiteSpace(userName))
                user = await _userManager.FindByNameAsync(userName);
            else
                user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                var error = string.IsNullOrWhiteSpace(userName) ? $"Пользователь с идентификатором {userId} отсутствует" : $"Пользователь с логином {userName} отсутствует";
                throw new AuthorizationResourceNotFoundException(error);
            }

            var rolesList = await _userManager.GetRolesAsync(user);

            return new UserEntry(user, rolesList);
        }

        public async Task<UserEntry> ConfirmEmail(string login, string code)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException($"Поля {nameof(login)},{nameof(code)} должны быть заполнены");
            }

            var user = await _userManager.FindByNameAsync(login);

            if (user == null)
            {
                throw new AuthorizationResourceNotFoundException($"Пользователь с логином {login} отсутствует");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                var rolesList = await _userManager.GetRolesAsync(user);

                return new UserEntry(user, rolesList);
            }
            else
                throw new AuthorizationException("Неверный код");
        }

        public async Task<(string name, string code)> ForgotPasswordAsync(ForgotPasswordModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                throw new AuthorizationException("Неправильно введен email");

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            return new(user.UserName, code);
        }

        public async Task ResetPasswordAsync(ResetPasswordModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new AuthorizationResourceNotFoundException("Пользователь не найден");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return;
            }

            throw new AuthorizationException("Во время сброса пароля произошла ошибка");
        }
    }
}
