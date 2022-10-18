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

namespace StuffyHelper.Authorization.Core.Features
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


            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = await user.CreateToken(_userManager, _authorizationConfiguration);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
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
                throw new EntityNotFoundException($"Пользователь с логином {model.Username} отсутствует");
            else
                throw new AuthorizationException($"Неверный пароль");
        }

        public async Task Logout(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        }

        public async Task<UserEntry> Register(RegisterModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            var userExists = await _userManager.FindByNameAsync(model.Username);

            if (userExists != null)
                throw new EntityAlreadyExistsException($"Пользователь с логином {model.Username} уже существует");

            StuffyUser identityUser = model.InitializeUser();

            var result = await _userManager.CreateAsync(identityUser, model.Password);

            if (!result.Succeeded)
                throw new AuthorizationException($"Ошибка создания пользователя! Детали: {string.Join(' ', result.Errors.Select(x => x.Description))}");

            await _roleManager.CreateRolesIfNotExists();
            await identityUser.AddRoleToUser(_roleManager, _userManager, UserType.User);
            var rolesList = await _userManager.GetRolesAsync(identityUser);

            return new UserEntry(identityUser, rolesList);
        }

        public IEnumerable<IdentityRole> GetRoles() => _roleManager.Roles.ToList();

        public bool? CheckUserIsAdmin(ClaimsPrincipal user) => user?.IsInRole(nameof(UserType.Admin));

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
                throw new EntityNotFoundException($"Пользователь с логином {userName} отсутствует");

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
                throw new EntityNotFoundException($"Пользователь с логином {user.Identity.Name} отсутствует");

            userToUpdate.PatchFrom(model);

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(userToUpdate);
                var passwordChangeResult = await _userManager.ResetPasswordAsync(userToUpdate, token, model.Password);
                passwordChangeResult.HandleIdentityResult();
            }

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
                throw new EntityNotFoundException(error);
            }

            var rolesList = await _userManager.GetRolesAsync(user);

            return new UserEntry(user, rolesList);
        }
    }
}
