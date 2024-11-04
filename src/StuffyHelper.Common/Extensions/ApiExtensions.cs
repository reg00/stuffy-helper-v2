using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace StuffyHelper.Common.Extensions;

/// <summary>
/// API extensions
/// </summary>
public static class ApiExtensions
{
    /// <summary>
    /// Add swagger support
    /// </summary>
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"Для авторизации в сваггере необходимо ввести токен, полученный при авторизации.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string>()
                }
            });

            //var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
        
        return services;
    }
}