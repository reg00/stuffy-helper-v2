using EnsureThat;
using StuffyHelper.Common.Middlewares;

namespace StuffyHelper.ApiGateway.Registration
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
