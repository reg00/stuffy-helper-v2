using EnsureThat;
using Microsoft.AspNetCore.Builder;
using StuffyHelper.Common.Middlewares;

namespace StuffyHelper.Common.Extensions
{
    /// <summary>
    /// Exception handling registration extensions
    /// </summary>
    public static class ExceptionHandlingMiddlewareExtensions
    {
        /// <summary>
        /// Add exception handling
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            EnsureArg.IsNotNull(builder);
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
