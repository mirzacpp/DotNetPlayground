using Microsoft.Extensions.Logging;
using System;

namespace Cleaners.Web.Infrastructure.Files
{
    public class CorvoIOExecutor : ICorvoIOExecutor
    {
        private readonly ILogger<CorvoIOExceptionHandler> _logger;

        public CorvoIOExecutor(ILogger<CorvoIOExceptionHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public T Execute<T>(Func<T> operation)
        {
            try
            {
                return operation();
            }
            catch (Exception ex)
            {
                // Create Exception handler service
                _logger.LogError(ex, "Error occured while executin IO operation");
                throw;
            }
        }

        public void Execute(Action operation)
        {
            throw new NotImplementedException();
        }

        CorvoFileResult ICorvoIOExecutor.Execute<T>(Func<T> operations)
        {
            throw new NotImplementedException();
        }
    }
}