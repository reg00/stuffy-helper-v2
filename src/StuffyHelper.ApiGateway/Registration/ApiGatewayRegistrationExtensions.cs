using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;
using StuffyHelper.Common.Middlewares;

namespace StuffyHelper.ApiGateway.Registration
{
    public static class ApiGatewayRegistrationExtensions
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
            
            return services;
        }

        public static IApplicationBuilder UseApi(this IApplicationBuilder app)
        {
            app.UseMiddleware<CorsHeaderMiddleware>();

            app.UseRouting();
            app.UseCors();

            app.UseAuth();

            return app;
        }

        private static IApplicationBuilder UseAuth(this IApplicationBuilder app)
        {
            app.UseAuthTokenChecker();
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
