using EnsureThat;
using Microsoft.AspNetCore.Http;

namespace StuffyHelper.Api.Features.Middlewares
{
    public class CorsHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public CorsHeaderMiddleware(RequestDelegate next)
        {
            EnsureArg.IsNotNull(next, nameof(next));

            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            EnsureArg.IsNotNull(context, nameof(context));
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            await _next(context);
        }
    }
}
