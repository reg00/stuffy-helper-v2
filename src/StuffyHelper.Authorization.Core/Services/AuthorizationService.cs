using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EnsureThat;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Authorization.Contracts.Enums;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Authorization.Core.Extensions;
using StuffyHelper.Authorization.Core.Services.Interfaces;
using StuffyHelper.Common.Configurations;
using StuffyHelper.Common.Exceptions;

namespace StuffyHelper.Authorization.Core.Services;

/// <inheritdoc />
public class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<StuffyUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AuthorizationConfiguration _authorizationConfiguration;
        private readonly IAvatarService _avatarService;

        /// <summary>
        /// Ctor.
        /// </summary>
        public AuthorizationService(
            UserManager<StuffyUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IAvatarService avatarService,
            IOptions<StuffyConfiguration> configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _avatarService = avatarService;
            _authorizationConfiguration = configuration.Value.Authorization;
        }

        /// <inheritdoc />
        public async Task<JwtSecurityToken> Login(LoginModel model, HttpContext httpContext)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            var user = await _userManager.FindByNameAsync(model.Username);

            if (user != null && !await _userManager.IsEmailConfirmedAsync(user))
                throw new ForbiddenException("Вы не подтвердили свой email");

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                var token = user.CreateToken(roles, _authorizationConfiguration);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.UserName ?? string.Empty));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName ?? string.Empty));

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
            else
                throw new EntityNotFoundException($"Неверный логин/пароль");
        }

        /// <inheritdoc />
        public async Task Logout(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        }

        /// <inheritdoc />
        public async Task<string> Register(RegisterModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            var userExists = await _userManager.FindByNameAsync(model.Username);

            if (userExists != null)
                throw new EntityAlreadyExistsException($"Пользователь с логином {model.Username} уже существует");

            var identityUser = model.InitializeUser();

            var result = await _userManager.CreateAsync(identityUser, model.Password);

            if (!result.Succeeded)
                throw new Exception($"Ошибка создания пользователя! Детали: {string.Join(' ', result.Errors.Select(x => x.Description))}");

            await _roleManager.CreateRolesIfNotExists();
            await identityUser.AddRoleToUser(_roleManager, _userManager, UserType.User);

            return await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
        }

        /// <inheritdoc />
        public IEnumerable<IdentityRole> GetRoles() => _roleManager.Roles.ToList();

        /// <inheritdoc />
        public async Task<bool> CheckUserIsAdmin(ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            var userName = user.Identity?.Name;

            if (userName == null)
                throw new ForbiddenException("Authorization error");

            var stuffyUser = await _userManager.FindByNameAsync(userName);

            if (stuffyUser == null)
                throw new EntityNotFoundException($"Cannot find user with name: {userName}");
            
            return await _userManager.IsInRoleAsync(stuffyUser, nameof(UserType.Admin));
        }

        /// <inheritdoc />
        public async Task<UserEntry> GetUserByToken(ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            var userName = user.Identity?.Name;

            if (userName == null)
                throw new ForbiddenException("Authorization error");

            var identityUser = await _userManager.FindByNameAsync(userName);
            
            if (identityUser == null)
                throw new EntityNotFoundException($"Cannot find user with name: {userName}");
            
            var rolesList = await _userManager.GetRolesAsync(identityUser);

            return new UserEntry(identityUser, rolesList);
        }

        /// <inheritdoc />
        public IEnumerable<UserShortEntry> GetUserLogins(string? userName = null)
        {
            var users = _userManager.Users
                .Where(u => userName == null || u.UserName != null && u.UserName.ToLower().StartsWith(userName.ToLower()) && u.EmailConfirmed == true)
                .Select(u => new UserShortEntry() { Id = u.Id, Name = u.UserName!, ImageUri = u.ImageUri }).ToList();

            return users;
        }

        /// <inheritdoc />
        public async Task DeleteUser(string? userName = null, string? userId = null)
        {
            if (string.IsNullOrWhiteSpace(userName) && string.IsNullOrWhiteSpace(userId))
                throw new Exception("UserName or UserId required");

            StuffyUser? userToDelete;

            if (!string.IsNullOrWhiteSpace(userName))
                userToDelete = await _userManager.FindByNameAsync(userName);
            else
                userToDelete = await _userManager.FindByIdAsync(userId!);

            if (userToDelete is null)
                throw new EntityNotFoundException($"Пользователь с логином {userName} отсутствует");

            var rolesList = await _userManager.GetRolesAsync(userToDelete);

            await _userManager.RemoveFromRolesAsync(userToDelete, rolesList);
            await _userManager.DeleteAsync(userToDelete);
        }

        /// <inheritdoc />
        public async Task<UserEntry> UpdateUser(ClaimsPrincipal user, UpdateModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));
            EnsureArg.IsNotNull(user, nameof(user));

            var userName = user.Identity?.Name;

            if (userName == null)
                throw new ForbiddenException("Authorization error");

            var userToUpdate = await _userManager.FindByNameAsync(userName);
            if (userToUpdate is null)
                throw new EntityNotFoundException($"Пользователь с логином {userName} отсутствует");

            userToUpdate.PatchFrom(model);

            var updateUserResult = await _userManager.UpdateAsync(userToUpdate);
            var updatedUser = await _userManager.FindByIdAsync(userToUpdate.Id);
            updateUserResult.HandleIdentityResult();
            
            if (updatedUser is null)
                throw new EntityNotFoundException($"Пользователь с Id {userToUpdate.Id} отсутствует");
            
            var rolesList = await _userManager.GetRolesAsync(updatedUser);

            return new UserEntry(updatedUser, rolesList);
        }

        /// <inheritdoc />
        public async Task UpdateAvatar(ClaimsPrincipal user, IFormFile file)
        {
            var userName = user.Identity?.Name;

            if (userName == null)
                throw new ForbiddenException("Authorization error");

            var existUser = await _userManager.FindByNameAsync(userName);
            if (existUser is null)
                throw new EntityNotFoundException($"Пользователь с логином {userName} отсутствует");

            if (file != null)
            {
                var addAvatar = new AddAvatarEntry(existUser.Id, file);

                await _avatarService.StoreAvatarFormFileAsync(addAvatar);

                var mediaUri = await _avatarService.GetAvatarUri(existUser.Id);
                existUser.ImageUri = mediaUri;
                await _userManager.UpdateAsync(existUser);
            }
        }

        /// <inheritdoc />
        public async Task RemoveAvatar(ClaimsPrincipal user)
        {
            var userName = user.Identity?.Name;

            if (userName == null)
                throw new ForbiddenException("Authorization error");

            var existUser = await _userManager.FindByNameAsync(userName);
            if (existUser is null)
                throw new EntityNotFoundException($"Пользователь с логином {userName} отсутствует");
            try
            {
                await _avatarService.DeleteAvatarAsync(existUser.Id);
            }
            catch (EntityNotFoundException)
            {

            }
        }

        /// <inheritdoc />
        public async Task<UserEntry> GetUserByName(string userName)
        {
            EnsureArg.IsNotEmptyOrWhiteSpace(userName, nameof(userName));

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                var error = $"Пользователь с логином {userName} отсутствует";
                throw new EntityNotFoundException(error);
            }

            var rolesList = await _userManager.GetRolesAsync(user);

            return new UserEntry(user, rolesList);
        }

        /// <inheritdoc />
        public async Task<UserEntry> GetUserById(string userId)
        {
            EnsureArg.IsNotEmptyOrWhiteSpace(userId, nameof(userId));

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                var error = $"Пользователь с идентификатором {userId} отсутствует";
                throw new EntityNotFoundException(error);
            }

            var rolesList = await _userManager.GetRolesAsync(user);

            return new UserEntry(user, rolesList);
        }

        /// <inheritdoc />
        public async Task<UserEntry> ConfirmEmail(string login, string code)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException($"Поля {nameof(login)},{nameof(code)} должны быть заполнены");
            }

            var user = await _userManager.FindByNameAsync(login);

            if (user == null)
            {
                throw new EntityNotFoundException($"Пользователь с логином {login} отсутствует");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                var rolesList = await _userManager.GetRolesAsync(user);

                return new UserEntry(user, rolesList);
            }
            else
                throw new ForbiddenException("Неверный код");
        }

        /// <inheritdoc />
        public async Task<(string name, string code)> ForgotPasswordAsync(ForgotPasswordModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                throw new ForbiddenException("Неправильно введен email");

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            return new(user.UserName ?? string.Empty, code);
        }

        /// <inheritdoc />
        public async Task ResetPasswordAsync(ResetPasswordModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new EntityNotFoundException("Пользователь не найден");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return;
            }

            throw new Exception("Во время сброса пароля произошла ошибка");
        }
    }