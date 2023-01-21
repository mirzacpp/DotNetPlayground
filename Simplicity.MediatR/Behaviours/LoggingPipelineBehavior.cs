using MediatR;

using Microsoft.Extensions.Logging;

namespace Simplicity.MediatR.Behaviours
{
    /// <summary>
    /// Enables logging pipeline for mediatR requests.
    /// TODO: Constraint TResponse with Result type...
    /// </summary>
    public class LoggingPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
        //where TResponse : FutureResultType
    {
        private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;

        public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            //Introduce logging messages
            _logger.LogInformation(
            "Starting request {RequestName} at {@DateTimeUtc} with payload {@Request}",
            typeof(TRequest).Name,
            request,
            DateTime.UtcNow);

            var result = await next();

            //if (result.) Log failure ...
            //{
            //}

            _logger.LogInformation(
          "Completed request {RequestName} at {@DateTimeUtc}",
            typeof(TRequest).Name,
            DateTime.UtcNow);

            return result;
        }
    }
}