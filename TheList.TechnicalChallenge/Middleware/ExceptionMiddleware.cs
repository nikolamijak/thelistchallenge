using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using System;
using TheList.TechnicalChallenge.Exceptions;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace TheList.TechnicalChallenge.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, ILogger<ExceptionMiddleware> _logger)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case CustomValidationException validationException:
                        var failedValidations = string.Join(",", validationException?.Failures?.Select(kv => kv.Key! + "=" + string.Join(";", kv.Value!))?.ToArray());
                        _logger.LogError(new EventId(500), ex, $"Validation Result: {failedValidations}");
                        break;
                    default:
                        _logger.LogError(new EventId(500), ex, "Handled Exception Occured");
                        break;
                }

                await HandleExceptionAsync(httpContext, ex);
                return;
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/vnd.api+json";

            var exceptionResponse = exception switch
            {
                CustomValidationException ex => new ExceptionResponse(new { code = ex.Code, reason = ex.Failures },
                    HttpStatusCode.BadRequest),
                CheckoutNotFoundException ex => new ExceptionResponse(new { code = ex.Code, reason = ex.Message },
                    HttpStatusCode.BadRequest),         
                _ => new ExceptionResponse(new { code = "error", reason = "There was an error." },
                    HttpStatusCode.InternalServerError)
            };

            context.Response.StatusCode = (int)(exceptionResponse?.StatusCode ?? HttpStatusCode.BadRequest);

            var response = exceptionResponse?.Response switch
            {
                null => context.Response.WriteAsync(string.Empty),
                _ => context.Response.WriteAsync(JsonSerializer.Serialize(exceptionResponse.Response))
            };

            await response;

        }
    }
}
