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
            
            // Customize response based on exception type
            switch (exception)
            {
                case InvalidEmailException:
                case InvalidMoneyAmountException:
                case InvalidMoneyCurrencyException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;
                case InvalidTenderDateException:
                case InvalidTenderOperationException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;
                case DuplicateTenderReferenceException:
                    statusCode = HttpStatusCode.Conflict; // 409 Conflict
                    message = exception.Message;
                    break;
                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = "The requested resource was not found.";
                    break;
                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = "You are not authorized to perform this action.";
                    break;
                // Add more exception types as needed
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(new 
            { 
                error = message,
                statusCode = (int)statusCode
            });
            
            await context.Response.WriteAsync(result);
        }
    }
} 