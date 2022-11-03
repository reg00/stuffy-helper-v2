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
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading;

namespace StuffyHelper.Authorization.Core.Features.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<StuffyUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<StuffyUser> _signInManager;
        private readonly AuthorizationConfiguration _authorizationConfiguration;

        public AuthorizationService(
            UserManager<StuffyUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<StuffyUser> signInManager,
            IOptions<AuthorizationConfiguration> authorizationConfiguration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _authorizationConfiguration = authorizationConfiguration.Value;
        }

        public async Task<JwtSecurityToken> Login(LoginModel model, HttpContext httpContext)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            var user = await _userManager.FindByNameAsync(model.Username);

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
            await _signInManager.SignOutAsync();
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<UserEntry> Register(RegisterModel model)
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
            var rolesList = await _userManager.GetRolesAsync(identityUser);

            return new UserEntry(identityUser, rolesList);
        }

        public IEnumerable<IdentityRole> GetRoles() => _roleManager.Roles.ToList();

        public async Task<bool> CheckUserIsAdmin(ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            var stuffyUser = await GetStuffyUser(user, cancellationToken);
            return await _userManager.IsInRoleAsync(stuffyUser, nameof(UserType.Admin));
        } 

        public async Task<UserEntry> GetAccountInfoAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            var stuffyUser = await GetStuffyUser(user, cancellationToken);
            var rolesList = await _userManager.GetRolesAsync(stuffyUser);

            return new UserEntry(stuffyUser, rolesList);
        }

        private async Task<StuffyUser> GetStuffyUser(ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(user, nameof(user));
            StuffyUser identityUser;

            var regularScheme = user.Identities.FirstOrDefault(x =>
                    x.AuthenticationType?.Equals("AuthenticationTypes.Federation") == true ||
                    x.AuthenticationType?.Equals("Cookies") == true);

            if (regularScheme is not null)
            {
                var login = regularScheme.Name;

                identityUser = await _userManager.FindByNameAsync(login);
            }
            else
            {
                var loginProvider = user.Identity?.AuthenticationType;
                var identifier = user.FindFirstValue(ClaimTypes.NameIdentifier);
                identityUser = await _userManager.FindByLoginAsync(loginProvider, identifier);
            }

            return identityUser;
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

            var userToUpdate = await GetStuffyUser(user);

            if (userToUpdate is null)
                throw new AuthorizationResourceNotFoundException($"Пользователь с логином {user.Identity.Name} отсутствует");

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
                throw new AuthorizationResourceNotFoundException(error);
            }

            var rolesList = await _userManager.GetRolesAsync(user);

            return new UserEntry(user, rolesList);
        }

        public AuthenticationProperties GoogleLogin(string redirectUrl)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
        }

        public async Task GoogleResponse()
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            
            if (info == null)
                throw new AuthorizationException("Could not find external info");

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

            if (result.Succeeded)
                return;
            else
            {
                StuffyUser user = new StuffyUser
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value
                };

                var identResult = await _userManager.CreateAsync(user);
                if (identResult.Succeeded)
                {
                    identResult = await _userManager.AddLoginAsync(user, info);
                    await user.AddRoleToUser(_roleManager, _userManager, UserType.User);

                    if (identResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);
                        return;
                    }
                }
                throw new AuthorizationException($"Exception while creating user. {identResult.Errors?.ElementAt(0).Description}");
            }
        }
    }
}
