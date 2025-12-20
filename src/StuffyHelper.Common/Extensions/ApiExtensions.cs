using System.Reflection;
using Microsoft.AspNetCore.Builder;
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

            var assembly = Assembly.GetEntryAssembly()?.GetName().Name;
            if (!string.IsNullOrWhiteSpace(assembly))
            {
                var xmlFilename = $"{assembly}.xml";
                var fullPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
                
                if (File.Exists(fullPath))
                    options.IncludeXmlComments(fullPath);
            }
        });
        
        return services;
    }

    /// <summary>
    /// Use swagger support
    /// </summary>
    public static IApplicationBuilder UseSwagger(this IApplicationBuilder builder, string name)
    {
        builder.UseSwagger(c =>
        {
            c.RouteTemplate = "api/swagger/{documentname}/swagger.json";
        });
        builder.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/api/swagger/v1/swagger.json", name);
            c.RoutePrefix = "swagger";
        });

        return builder;
    }
}