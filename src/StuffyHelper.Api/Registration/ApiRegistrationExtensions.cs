using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StuffyHelper.Api.Features.Middlewares;
using StuffyHelper.Authorization.Core.Registration;
using StuffyHelper.Authorization.EntityFrameworkCore.Registration;
using StuffyHelper.EntityFrameworkCore.Registration;
using StuffyHelper.Minio.Registration;
using System.Reflection;
using System.Text.Json.Serialization;

namespace StuffyHelper.Api.Registration
{
    public static class ApiRegistrationExtensions
    {
        public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });

            services.AddAuth(configuration);

            services
                .AddControllers(options => options.UseDateOnlyTimeOnlyStringConverters())
                .AddJsonOptions(options =>
                {
                    options.UseDateOnlyTimeOnlyStringConverters();
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                 });

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.UseCamelCasing(true);
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });
            services.AddControllersWithViews();
            services.AddEndpointsApiExplorer();
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

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            services.AddMinioBlobDataStores(configuration);

            return services;
        }

        public static IApplicationBuilder UseApi(this IApplicationBuilder app)
        {
            app.UseMiddleware<CorsHeaderMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();
            app.UseCors();

            app.UseAuth();

            app.UseEndpoints(options =>
            {
                options.MapControllers();
            });

            return app;
        }

        private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEfAuthDbServices(configuration);
            services.AddJwtAuthentication(configuration);
            services.AddAuthorization();

            return services;
        }

        private static IApplicationBuilder UseAuth(this IApplicationBuilder app)
        {
            app.ApplicationServices.AddAuthDatabaseMigration();
            app.ApplicationServices.AddEfDatabaseMigration();

            app.UseAuthTokenChecker();
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
