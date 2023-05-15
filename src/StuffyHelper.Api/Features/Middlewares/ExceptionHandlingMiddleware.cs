using EnsureThat;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reg00.Infrastructure.Errors;
using StuffyHelper.Authorization.Core.Exceptions;
using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Common;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.ExceptionServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StuffyHelper.Api.Features.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            EnsureArg.IsNotNull(next, nameof(next));
            EnsureArg.IsNotNull(logger, nameof(logger));

            _next = next;
            _logger = logger;
            jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true
            };
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

                var exceptionDispatchInfo = ExceptionDispatchInfo.Capture(exception);
                if (exceptionDispatchInfo != null)
                {
                    var statusCode = MapStatusCodeToResult(exceptionDispatchInfo.SourceException);
                    context.Response.ContentType = new MediaTypeHeaderValue("application/json").ToString();
                    context.Response.StatusCode = (int)statusCode;
                    var result = JsonSerializer.Serialize(new ErrorResponse() { Message = exception.Message }, jsonSerializerOptions);
                    await context.Response.WriteAsync(result);
                }
            }
        }

        public HttpStatusCode MapStatusCodeToResult(Exception exception)
        {
            EnsureArg.IsNotNull(exception, nameof(exception));

            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = exception.Message;

            // TODO: Register all possible exceptions in exception handler middleware.

            switch (exception)
            {
                case ValidationException _:
                case System.NotSupportedException _:
                case Reg00.Infrastructure.Errors.NotSupportedException _:
                case ArgumentNullException _:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                case EntityNotFoundException _:
                    statusCode = HttpStatusCode.NotFound;
                    break;
                case ForbiddenException _:
                    statusCode = HttpStatusCode.Forbidden;
                    break;
                case DbStoreException _:
                case StuffyException _:
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
                case EntityAlreadyExistsException _:
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

            return statusCode;
        }

        protected internal virtual async Task ExecuteResultAsync(HttpContext context, IActionResult result)
        {
            EnsureArg.IsNotNull(context, nameof(context));
            EnsureArg.IsNotNull(result, nameof(result));
            await result.ExecuteResultAsync(new ActionContext { HttpContext = context });
        }
    }
}
