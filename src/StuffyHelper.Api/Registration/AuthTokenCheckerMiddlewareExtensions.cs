using EnsureThat;
using Microsoft.AspNetCore.Builder;
using StuffyHelper.Api.Features.Middlewares;

namespace StuffyHelper.Api.Registration
{
    public static class AuthTokenCheckerMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthTokenChecker(this IApplicationBuilder builder)
        {
            EnsureArg.IsNotNull(builder);
            return builder.UseMiddleware<AuthTokenCheckerMiddleware>();
        }
    }
}
