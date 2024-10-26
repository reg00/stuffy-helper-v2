using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using StuffyHelper.Authorization.Core1.Features;

namespace StuffyHelper.Authorization.Core1.Registration
{
    public static class AuthorizationExtensions
    {
        public static IApplicationBuilder SeedUserData(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var initializer = scope.ServiceProvider.GetRequiredService<IInitializer>();
            initializer.Initialize();

            return app;
        }
    }
}
