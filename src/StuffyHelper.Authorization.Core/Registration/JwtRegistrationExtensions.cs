using EnsureThat;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using StuffyHelper.Authorization.Core.Configs;
using StuffyHelper.Authorization.Core.Features;
using StuffyHelper.Authorization.Core.Features.Authorization;
using StuffyHelper.Authorization.Core.Features.FriendsRequest;
using System.Text;

namespace StuffyHelper.Authorization.Core.Registration
{
    public static class JwtRegistrationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            EnsureArg.IsNotNull(configuration, nameof(configuration));

            var authorizationOptions = new AuthorizationConfiguration();
            configuration.GetSection(AuthorizationConfiguration.DefaultSectionName)
                .Bind(authorizationOptions);

            services.AddSingleton(Options.Create(authorizationOptions));

            EnsureArg.IsNotNull(authorizationOptions, nameof(authorizationOptions));

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "JWT_OR_COOKIE";
                options.DefaultChallengeScheme = "JWT_OR_COOKIE";
                //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie("Cookies", options =>
            {
                options.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = async (context) =>
                    {
                        //context.HttpContext.Response.Redirect("api/auth/login");
                    }
                };
                options.LoginPath = "/api/auth/login";
                options.LogoutPath = "/api/auth/logout";
                options.AccessDeniedPath = "/api/auth/login";
                options.ExpireTimeSpan = TimeSpan.FromHours(authorizationOptions.JWT.TokenExpireInHours);
            })
            .AddJwtBearer("Bearer", options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = authorizationOptions.JWT.ValidAudience,
                    ValidIssuer = authorizationOptions.JWT.ValidIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authorizationOptions.JWT.Secret))
                };
            })
            .AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    // filter by auth type
                    string authorization = context.Request.Headers[HeaderNames.Authorization];
                    if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                        return "Bearer";

                    // otherwise always check for cookie auth
                    return "Cookies";
                };
            });

            services.AddAuthentificationServices();

            return services;
        }

        public static IServiceCollection AddAuthentificationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IFriendsRequestService, FriendsRequestService>();

            return services;
        }
    }
}
