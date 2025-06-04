using EnsureThat;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StuffyHelper.Common.Configurators;

namespace StuffyHelper.Common.Extensions;

/// <summary>
/// Authorization extensions
/// </summary>
public static class AuthorizationExtensions
{
    /// <summary>
    /// Add stuffy helper authorization 
    /// </summary>
    public static IServiceCollection AddStuffyAuthorization(this IServiceCollection services,
        IConfiguration configuration)
    {
        var config = configuration.GetConfig().Authorization;

        EnsureArg.IsNotNull(config, nameof(config));
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = config.JWT.ValidAudience,
                    ValidIssuer = config.JWT.ValidIssuer,
                    IssuerSigningKey = config.JWT.GetSecurityKey()
                };
            });

        services.AddAuthorization(options =>
        {
            var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                JwtBearerDefaults.AuthenticationScheme);

            defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
            options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
        });

        return services;
    }
}