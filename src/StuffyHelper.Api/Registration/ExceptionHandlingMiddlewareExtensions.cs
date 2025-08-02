using EnsureThat;
using StuffyHelper.Api.Middlewares;

namespace StuffyHelper.Api.Registration
{
    /// <summary>
    /// Exception handling registration extensions
    /// </summary>
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            EnsureArg.IsNotNull(builder);
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
