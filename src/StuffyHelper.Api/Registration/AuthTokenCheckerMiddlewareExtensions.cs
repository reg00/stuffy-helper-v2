using EnsureThat;
using StuffyHelper.Api.Middlewares;

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
