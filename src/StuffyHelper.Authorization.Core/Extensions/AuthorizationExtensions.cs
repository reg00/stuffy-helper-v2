using EnsureThat;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StuffyHelper.Authorization.Core.Configs;
using StuffyHelper.Authorization.Core.Exceptions;
using StuffyHelper.Authorization.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StuffyHelper.Authorization.Core.Extensions
{
    public static class AuthorizationExtensions
    {
        private static JwtSecurityToken GetToken(this List<Claim> authClaims, AuthorizationConfiguration authorizationConfiguration)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authorizationConfiguration.JWT.Secret));

            var token = new JwtSecurityToken(
                issuer: authorizationConfiguration.JWT.ValidIssuer,
                audience: authorizationConfiguration.JWT.ValidAudience,
                expires: DateTime.Now.AddHours(authorizationConfiguration.JWT.TokenExpireInHours),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        public static async Task<JwtSecurityToken> CreateToken(this StuffyUser user, UserManager<StuffyUser> _userManager, AuthorizationConfiguration authorizationConfiguration)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));

            return authClaims.GetToken(authorizationConfiguration);
        }

        public static async Task CreateRolesIfNotExists(this RoleManager<IdentityRole> _roleManager)
        {
            if (!await _roleManager.RoleExistsAsync(nameof(UserType.Admin)))
                await _roleManager.CreateAsync(new IdentityRole(nameof(UserType.Admin)));
            if (!await _roleManager.RoleExistsAsync(nameof(UserType.User)))
                await _roleManager.CreateAsync(new IdentityRole(nameof(UserType.User)));
        }

        public static async Task AddRoleToUser(this StuffyUser user, RoleManager<IdentityRole> _roleManager, UserManager<StuffyUser> _userManager, UserType role)
        {
            EnsureArg.IsNotNull(user, nameof(user));
            EnsureArg.IsNotNull(_roleManager, nameof(_roleManager));
            EnsureArg.IsNotNull(_userManager, nameof(_userManager));

            if (role == UserType.Admin && await _roleManager.RoleExistsAsync(nameof(UserType.Admin)))
                await _userManager.AddToRoleAsync(user, nameof(UserType.Admin));

            if (await _roleManager.RoleExistsAsync(nameof(UserType.User)))
                await _userManager.AddToRoleAsync(user, nameof(UserType.User));
        }

        public static StuffyUser InitializeUser(this RegisterModel model)
        {
            return new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                PhoneNumber = model.Phone,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                FirstName = model.FirstName
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
}
