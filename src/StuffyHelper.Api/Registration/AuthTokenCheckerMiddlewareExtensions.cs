using EnsureThat;
using StuffyHelper.Common.Middlewares;

namespace StuffyHelper.Api.Registration
{
    /// <summary>
    /// Auth middleware registration extensions
    /// </summary>
    public static class AuthTokenCheckerMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthTokenChecker(this IApplicationBuilder builder)
        {
            EnsureArg.IsNotNull(builder);
            return builder.UseMiddleware<AuthTokenCheckerMiddleware>();
        }
    }
}
