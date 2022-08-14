using EnsureThat;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StuffyHelper.Authorization.Core.Configs;
using StuffyHelper.Authorization.Core.Features;
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

            services.AddSingleton(Microsoft.Extensions.Options.Options.Create(authorizationOptions));

            EnsureArg.IsNotNull(authorizationOptions, nameof(authorizationOptions));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
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

            services.AddAuthentificationServices();

            return services;
        }

        public static IServiceCollection AddAuthentificationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationService, AuthorizationService>();

            return services;
        }
    }
}
