using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StuffyHelper.Authorization.Core.Registration;
using StuffyHelper.Authorization.EntityFrameworkCore.Registration;

namespace StuffyHelper.Api.Registration
{
    public static class ApiRegistrationExtensions
    {
        public static IServiceCollection AddEckApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEckAuth(configuration);

            services
                .AddControllers(options => options.UseDateOnlyTimeOnlyStringConverters())
                .AddJsonOptions(options => options.UseDateOnlyTimeOnlyStringConverters());

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
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
            });

            return services;
        }

        public static IApplicationBuilder UseEckApi(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();

            app.UseEckAuth();

            app.UseEndpoints(options =>
            {
                options.MapControllers();
            });

            return app;
        }

        private static IServiceCollection AddEckAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEfAuthDbServices(configuration);
            services.AddJwtAuthentication(configuration);
            services.AddAuthorization();

            return services;
        }

        private static IApplicationBuilder UseEckAuth(this IApplicationBuilder app)
        {
            app.ApplicationServices.AddEfDatabaseMigration();

            app.UseAuthTokenChecker();
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
