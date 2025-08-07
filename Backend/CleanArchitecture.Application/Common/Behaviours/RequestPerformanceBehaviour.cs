using System.Diagnostics;
using CleanArchitecture.Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Common.Behaviours
{
    public class RequestPerformanceBehaviour<TRequest, TResponse>(
        ILogger<TRequest> logger,
        ICurrentUserService currentUserService,
        IHttpContextAccessor httpContextAccessor)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly Stopwatch _timer = new();

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            _timer.Start();

            var response = await next(cancellationToken);

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;
            var requestName = typeof(TRequest).Name;
            var userId = currentUserService.UserId;
            var userName = currentUserService.UserName;

            var requestPath = httpContextAccessor.HttpContext?.Request.Path.ToString();
            var clientIp = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

            if (elapsedMilliseconds > 500)
            {
                logger.LogWarning(
                    "Api Long Running Request: {Name} ({ElapsedMilliseconds} ms) UserId: {UserId}, UserName: {UserName}, Request: {@Request} {RequestPath} {ClientIp} {SourceContext}",
                    requestName, elapsedMilliseconds, userId, userName, request, requestPath, clientIp, typeof(RequestPerformanceBehaviour<TRequest, TResponse>).FullName);
            }
            else
            {
                logger.LogInformation(
                    "Api Running Request: {Name} ({ElapsedMilliseconds} ms) UserId: {UserId}, UserName: {UserName}, Request: {@Request} {RequestPath} {ClientIp} {SourceContext}",
                    requestName, elapsedMilliseconds, userId, userName, request, requestPath, clientIp, typeof(RequestPerformanceBehaviour<TRequest, TResponse>).FullName);
            }

            return response;
        }
    }
}
