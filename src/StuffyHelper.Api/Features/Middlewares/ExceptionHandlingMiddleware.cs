using EnsureThat;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StuffyHelper.Authorization.Core.Exceptions;
using StuffyHelper.Core.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Runtime.ExceptionServices;

namespace StuffyHelper.Api.Features.Middlewares
{
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
            ExceptionDispatchInfo exceptionDispatchInfo = null;
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

                // Get the Exception, but don't continue processing in the catch block as its bad for stack usage.
                exceptionDispatchInfo = ExceptionDispatchInfo.Capture(exception);
            }

            if (exceptionDispatchInfo != null)
            {
                IActionResult result = MapExceptionToResult(exceptionDispatchInfo.SourceException);
                await ExecuteResultAsync(context, result);
            }
        }

        public IActionResult MapExceptionToResult(Exception exception)
        {
            EnsureArg.IsNotNull(exception, nameof(exception));

            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = exception.Message;

            // TODO: Register all possible exceptions in exception handler middleware.

            switch (exception)
            {
                case ValidationException _:
                case NotSupportedException _:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                case ResourceNotFoundException _:
                case Core.Exceptions.EntityNotFoundException _:
                case Authorization.Core.Exceptions.EntityNotFoundException _:
                    statusCode = HttpStatusCode.NotFound;
                    break;
                case AuthStoreException _:
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
                case Core.Exceptions.EntityAlreadyExistsException _:
                case Authorization.Core.Exceptions.EntityAlreadyExistsException _:
                case EntityConflictException _:
                    statusCode = HttpStatusCode.Conflict;
                    break;
                case UnauthorizedAccessException _:
                case AuthorizationException _:
                    statusCode = HttpStatusCode.Unauthorized;
                    break;
            }

            // Log the exception and possibly modify the user message
            switch (statusCode)
            {
                case HttpStatusCode.ServiceUnavailable:
                    _logger.LogWarning(exception, "Service exception.");
                    break;
                case HttpStatusCode.InternalServerError:
                    _logger.LogCritical(exception, "Unexpected service exception.");
                    break;
                default:
                    _logger.LogWarning(exception, "Unhandled exception");
                    break;
            }

            return GetContentResult(statusCode, message);
        }

        private static IActionResult GetContentResult(HttpStatusCode statusCode, string message)
        {
            return new ContentResult
            {
                StatusCode = (int)statusCode,
                Content = message,
            };
        }

        protected internal virtual async Task ExecuteResultAsync(HttpContext context, IActionResult result)
        {
            EnsureArg.IsNotNull(context, nameof(context));
            EnsureArg.IsNotNull(result, nameof(result));
            await result.ExecuteResultAsync(new ActionContext { HttpContext = context });
        }
    }
}
