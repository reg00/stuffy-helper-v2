using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EnsureThat;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Authorization.Contracts.Enums;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Configurations;
using StuffyHelper.Common.Exceptions;

namespace StuffyHelper.Authorization.Core.Extensions;

public static class AuthorizationExtensions
    {
        private static JwtSecurityToken GetToken(this List<Claim> authClaims, AuthorizationConfiguration authorizationConfiguration)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authorizationConfiguration.JWT.Secret));

            var token = new JwtSecurityToken(
                issuer: authorizationConfiguration.JWT.ValidIssuer,
                audience: authorizationConfiguration.JWT.ValidAudience,
                expires: DateTime.UtcNow.AddHours(authorizationConfiguration.JWT.TokenExpireInHours),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        public static JwtSecurityToken CreateToken(this StuffyUser user, IEnumerable<string> roles, AuthorizationConfiguration authorizationConfiguration)
        {
            var authClaims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.UserName ?? string.Empty),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in roles)
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));

            return authClaims.GetToken(authorizationConfiguration);
        }

        public static async Task CreateRolesIfNotExists(this RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(nameof(UserType.Admin)))
                await roleManager.CreateAsync(new IdentityRole(nameof(UserType.Admin)));
            if (!await roleManager.RoleExistsAsync(nameof(UserType.User)))
                await roleManager.CreateAsync(new IdentityRole(nameof(UserType.User)));
        }

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

        public static StuffyUser InitializeUser(this RegisterModel model)
        {
            return new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
            };
        }


        public static void HandleIdentityResult(this IdentityResult identityResult)
        {
            if (!identityResult.Succeeded)
            {
                var errors = string.Join(' ', identityResult.Errors.Select(y => y.Description));

                if (identityResult.Errors.Any(x => x.Code == "DuplicateUserName"))
                    throw new EntityAlreadyExistsException(errors);

                throw new AuthorizationException($"Ошибка обновления пользователя!. Детали: {errors}");
            }
        }
    }