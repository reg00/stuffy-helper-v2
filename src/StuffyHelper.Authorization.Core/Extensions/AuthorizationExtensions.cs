using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EnsureThat;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Authorization.Contracts.Enums;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Configurations;
using StuffyHelper.Common.Exceptions;

namespace StuffyHelper.Authorization.Core.Extensions;

/// <summary>
/// Authorization extensions
/// </summary>
public static class AuthorizationExtensions
    {
        /// <summary>
        /// Get JwtSecurityToken
        /// </summary>
        private static JwtSecurityToken GetToken(this List<Claim> authClaims, AuthorizationConfiguration authorizationConfiguration)
        {
            var token = new JwtSecurityToken(
                issuer: authorizationConfiguration.JWT.ValidIssuer,
                audience: authorizationConfiguration.JWT.ValidAudience,
                expires: DateTime.UtcNow.AddHours(authorizationConfiguration.JWT.TokenExpireInHours),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authorizationConfiguration.JWT.GetSecurityKey(), SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        /// <summary>
        /// Creates JWT token
        /// </summary>
        public static JwtSecurityToken CreateToken(this StuffyUser user, IEnumerable<string> roles, AuthorizationConfiguration authorizationConfiguration)
        {
            var authClaims = new List<Claim>
                {
                    new (ClaimTypes.Sid, user.Id),
                    new(ClaimTypes.Name, user.UserName ?? string.Empty),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new(ClaimTypes.Uri, user.ImageUri?.AbsoluteUri ?? string.Empty)
                };

            foreach (var userRole in roles)
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));

            return authClaims.GetToken(authorizationConfiguration);
        }

        /// <summary>
        /// Create role if not exists
        /// </summary>
        public static async Task CreateRolesIfNotExists(this RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(nameof(UserType.Admin)))
                await roleManager.CreateAsync(new IdentityRole(nameof(UserType.Admin)));
            if (!await roleManager.RoleExistsAsync(nameof(UserType.User)))
                await roleManager.CreateAsync(new IdentityRole(nameof(UserType.User)));
        }

        /// <summary>
        /// Add role to user
        /// </summary>
        public static async Task AddRoleToUser(this StuffyUser user, RoleManager<IdentityRole> roleManager, UserManager<StuffyUser> userManager, UserType role)
        {
            EnsureArg.IsNotNull(user, nameof(user));
            EnsureArg.IsNotNull(roleManager, nameof(roleManager));
            EnsureArg.IsNotNull(userManager, nameof(userManager));

            if (role == UserType.Admin && await roleManager.RoleExistsAsync(nameof(UserType.Admin)))
                await userManager.AddToRoleAsync(user, nameof(UserType.Admin));

            if (await roleManager.RoleExistsAsync(nameof(UserType.User)))
                await userManager.AddToRoleAsync(user, nameof(UserType.User));
        }

        /// <summary>
        /// Initialize user
        /// </summary>
        public static StuffyUser InitializeUser(this RegisterModel model)
        {
            return new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
            };
        }


        /// <summary>
        /// Handle identity result
        /// </summary>
        public static void HandleIdentityResult(this IdentityResult identityResult)
        {
            if (identityResult.Succeeded) return;
            
            var errors = string.Join(' ', identityResult.Errors.Select(y => y.Description));

            if (identityResult.Errors.Any(x => x.Code == "DuplicateUserName"))
                throw new EntityAlreadyExistsException(errors);

            throw new AuthorizationException($"Ошибка обновления пользователя!. Детали: {errors}");
        }
    }