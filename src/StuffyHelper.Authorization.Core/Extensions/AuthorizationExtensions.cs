using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EnsureThat;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
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
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(authorizationConfiguration.JWT.TokenExpireInHours),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authorizationConfiguration.JWT.GetSecurityKey(), SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        /// <summary>
        /// Creates JWT token
        /// </summary>
        public static (string, DateTime) CreateAccessToken(this StuffyUser user, IEnumerable<string> roles, AuthorizationConfiguration authorizationConfiguration)
        {
            var now = DateTime.UtcNow;
            var exp = now.AddHours(authorizationConfiguration.JWT.TokenExpireInHours);
            
            var authClaims = new List<Claim>
                {
                    new (ClaimTypes.Sid, user.Id),
                    new(ClaimTypes.Name, user.UserName ?? string.Empty),
                    new Claim(JwtRegisteredClaimNames.Iat, ((long)(now - DateTime.UnixEpoch).TotalSeconds).ToString(), ClaimValueTypes.Integer64),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new(ClaimTypes.Uri, user.ImageUri?.AbsoluteUri ?? string.Empty)
                };

            foreach (var userRole in roles)
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));

            var token = authClaims.GetToken(authorizationConfiguration);
            return (new JwtSecurityTokenHandler().WriteToken(token), exp);
        }

        /// <summary>
        /// Creates refresh token entity
        /// </summary>
        public static RefreshEntity CreateRefreshToken(string userId)
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            var token = WebEncoders.Base64UrlEncode(bytes);
            
            return new RefreshEntity()
            {
                Id =  Guid.NewGuid(),
                UserId = userId,
                Hash = HashToken(token),
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                CreatedAt = DateTime.UtcNow,
                Revoked = false
            };
        }
        
        /// <summary>
        /// Hash token
        /// </summary>
        private static string HashToken(string token)
        {
            // В хранилище сохраняем только hash RT
            return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
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
                throw new EntityAlreadyExistsException("Username already exists. Details : {Details}", errors);

            throw new DbStoreException("Error while updating user!. Details: {Details}", errors);
        }
    }