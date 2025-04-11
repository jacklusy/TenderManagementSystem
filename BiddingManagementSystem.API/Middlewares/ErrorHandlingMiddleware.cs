using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using BiddingManagementSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BiddingManagementSystem.API.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError; // 500 if unexpected
            var message = "An unexpected error occurred.";
            var detail = exception.Message;
            
            // Customize response based on exception type
            switch (exception)
            {
                case InvalidOperationException ioe when ioe.Message.Contains("No service for type"):
                    message = "A required service handler was not found. This is likely a configuration issue.";
                    detail = "Please ensure all handlers are properly registered: " + ioe.Message;
                   // _logger.LogCritical(ioe, "Missing dependency: {Error}", ioe.Message);
                    break;
                case InvalidEmailException:
                case InvalidMoneyAmountException:
                case InvalidMoneyCurrencyException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "Invalid input data.";
                    detail = exception.Message;
                    break;
                case InvalidTenderDateException:
                case InvalidTenderOperationException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "Invalid tender operation.";
                    detail = exception.Message;
                    break;
                case DuplicateTenderReferenceException:
                    statusCode = HttpStatusCode.Conflict; // 409 Conflict
                    message = "A duplicate tender reference was detected.";
                    detail = exception.Message;
                    break;
                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = "The requested resource was not found.";
                    detail = exception.Message;
                    break;
                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = "You are not authorized to perform this action.";
                    detail = exception.Message;
                    break;
                // Add more exception types as needed
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(new 
            { 
                error = message,
                detail = detail,
                statusCode = (int)statusCode
            });
            
            await context.Response.WriteAsync(result);
        }
    }
} 