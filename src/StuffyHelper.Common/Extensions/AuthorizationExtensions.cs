using EnsureThat;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StuffyHelper.Common.Configurators;

namespace StuffyHelper.Common.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddStuffyAuthorization(this IServiceCollection services,
        IConfiguration configuration)
    {
        var config = configuration.GetConfig().Authorization;

        EnsureArg.IsNotNull(config, nameof(config));
        
        services.AddAuthentication()
            .AddCookie(options =>
            {
                options.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = async (_) =>
                    {
                        await Task.CompletedTask;
                        //context.HttpContext.Response.Redirect("api/auth/login");
                    }
                };
                options.LoginPath = "/api/auth/login";
                options.LogoutPath = "/api/auth/logout";
                options.AccessDeniedPath = "/api/auth/login";
                options.ExpireTimeSpan = TimeSpan.FromHours(config.JWT.TokenExpireInHours);
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = config.JWT.ValidAudience,
                    ValidIssuer = config.JWT.ValidIssuer,
                    IssuerSigningKey = config.JWT.GetSecurityKey()
                };
            });

        services.AddAuthorization(options =>
        {
            var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                CookieAuthenticationDefaults.AuthenticationScheme,
                JwtBearerDefaults.AuthenticationScheme);

            defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
            options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
        });

        return services;
    }
}