using EnsureThat;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StuffyHelper.Common.Exceptions;

namespace StuffyHelper.Common.Middlewares
{
    /// <summary>
    /// Exception handling middleware
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            EnsureArg.IsNotNull(next, nameof(next));
            EnsureArg.IsNotNull(logger, nameof(logger));

            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            EnsureArg.IsNotNull(context, nameof(context));

            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the base exception middleware will not be executed.");
                    throw;
                }

                await HandleExceptionAsync(context, exception, context.RequestAborted);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, CancellationToken cancellationToken = default)
        {
            if (exception is BaseException baseException)
            {
                // Логируем с исходным шаблоном и значениями
                _logger.LogWarning(
                    "Exception: {ErrorCode}, Template: {MessageTemplate}, Args: {Args}",
                    baseException.ErrorCode,
                    baseException.MessageTemplate,
                    baseException.Args);

                context.Response.StatusCode = (int)baseException.HttpStatus;
                context.Response.ContentType = "application/json";
            
                await context.Response.WriteAsJsonAsync(
                    baseException.ToApiError(), 
                    cancellationToken);

                return;
            }

            // Обработка других типов исключений
            _logger.LogError(exception, "Unhandled exception occurred");

            var genericError = new
            {
                errorCode = "UNEXPECTED_ERROR",
                message = "An unexpected error occurred",
                httpStatus = (int)HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
        
            await context.Response.WriteAsJsonAsync(genericError, cancellationToken);
        }
    }
}
