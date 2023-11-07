using EnsureThat;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StuffyHelper.Authorization.Core.Configs;
using StuffyHelper.Authorization.Core.Features.Authorization;
using StuffyHelper.Authorization.Core.Features.Friends;
using StuffyHelper.Authorization.Core.Features.Friend;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using IAuthorizationService = StuffyHelper.Authorization.Core.Features.IAuthorizationService;
using StuffyHelper.Authorization.Core.Features.Avatar;

namespace StuffyHelper.Authorization.Core.Registration
{
    public static class JwtRegistrationExtensions
    {
        public static IServiceCollection AddStuffyAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            EnsureArg.IsNotNull(configuration, nameof(configuration));

            var authorizationOptions = new AuthorizationConfiguration();
            configuration.GetSection(AuthorizationConfiguration.DefaultSectionName)
                .Bind(authorizationOptions);

            services.AddSingleton(Options.Create(authorizationOptions));

            EnsureArg.IsNotNull(authorizationOptions, nameof(authorizationOptions));

            services.AddAuthentication()
            .AddCookie(options =>
            {
                options.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = async (context) =>
                    {
                        await Task.CompletedTask;
                        //context.HttpContext.Response.Redirect("api/auth/login");
                    }
                };
                options.LoginPath = "/api/auth/login";
                options.LogoutPath = "/api/auth/logout";
                options.AccessDeniedPath = "/api/auth/login";
                options.ExpireTimeSpan = TimeSpan.FromHours(authorizationOptions.JWT.TokenExpireInHours);
            })
            .AddJwtBearer(options =>
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
            });

            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    JwtBearerDefaults.AuthenticationScheme);

                defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });

            services.AddAuthentificationServices();

            return services;
        }

        public static IServiceCollection AddAuthentificationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IFriendsRequestService, FriendsRequestService>();
            services.AddScoped<IFriendService, FriendService>();
            services.AddScoped<IAvatarService, AvatarService>();

            return services;
        }
    }
}
