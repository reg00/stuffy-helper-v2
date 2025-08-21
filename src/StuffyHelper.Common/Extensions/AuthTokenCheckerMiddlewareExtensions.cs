using EnsureThat;
using Microsoft.AspNetCore.Builder;
using StuffyHelper.Common.Middlewares;

namespace StuffyHelper.Common.Extensions
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
