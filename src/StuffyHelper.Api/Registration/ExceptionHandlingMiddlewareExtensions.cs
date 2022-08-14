using EnsureThat;
using Microsoft.AspNetCore.Builder;
using StuffyHelper.Api.Features.Middlewares;

namespace StuffyHelper.Api.Registration
{
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            EnsureArg.IsNotNull(builder);
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
