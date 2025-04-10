using System;
using System.Threading;
using System.Threading.Tasks;
using BiddingManagementSystem.Application.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BiddingManagementSystem.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        private readonly ICurrentUserService _currentUserService;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = request.GetType().Name;
            var userId = _currentUserService.UserId ?? "Anonymous";
            var userName = _currentUserService.Username ?? "Anonymous";

            _logger.LogInformation(
                "Handling {RequestName} for user {UserId} ({UserName})",
                requestName, userId, userName);

            var timer = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                var response = await next();
                timer.Stop();

                _logger.LogInformation(
                    "Handled {RequestName} for user {UserId} ({UserName}) in {ElapsedMilliseconds}ms",
                    requestName, userId, userName, timer.ElapsedMilliseconds);

                return response;
            }
            catch (Exception ex)
            {
                timer.Stop();

                _logger.LogError(
                    ex, "Error handling {RequestName} for user {UserId} ({UserName}) after {ElapsedMilliseconds}ms",
                    requestName, userId, userName, timer.ElapsedMilliseconds);

                throw;
            }
        }
    }
} 