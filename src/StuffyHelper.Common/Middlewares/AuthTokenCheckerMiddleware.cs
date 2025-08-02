using Microsoft.AspNetCore.Http;

namespace StuffyHelper.Common.Middlewares
{
    public class AuthTokenCheckerMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthTokenCheckerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Query["access_token"];

            if (string.IsNullOrEmpty(context.Request.Headers["Authorization"]) && !string.IsNullOrEmpty(token))
            {
                context.Request.Headers["Authorization"] = $"Bearer {token}";
            }

            await _next(context);
        }
    }
}
