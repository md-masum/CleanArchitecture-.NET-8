using CleanArchitecture.Application.Interfaces.Services;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Common.Behaviours
{
    public class RequestLogger<TRequest>(ILogger<TRequest> logger, ICurrentUserService currentUserService) : IRequestPreProcessor<TRequest> where TRequest : notnull
    {
        private readonly ILogger _logger = logger;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.UserId;
            var userName = _currentUserService.UserName;

            //var logToDatabase = new RequestLoggerEntity
            //{
            //    RequestName = requestName,
            //    UserId = userId,
            //    UserName = userName
            //};

            //await _logToDatabaseService.Save(logToDatabase, cancellationToken);

            _logger.LogInformation("Api Request: {Name} {@UserId} {@UserName} {@Request} {SourceContext}",
            requestName, userId, userName, request, typeof(RequestLogger<TRequest>).FullName);
            return Task.CompletedTask;
        }
    }
}
