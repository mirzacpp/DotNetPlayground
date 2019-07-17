using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;

namespace Cleaners.Web.Infrastructure.Files
{
    /// <summary>
    /// Default exception handler for Corvo IO provider
    /// More info at https://docs.microsoft.com/en-us/dotnet/standard/io/handling-io-errors
    /// <seealso cref="http://www.tech-archive.net/Archive/DotNet/microsoft.public.dotnet.languages.csharp/2007-08/msg02156.html"/>
    /// </summary>
    public class CorvoIOExceptionHandler
    {
        private readonly ILogger<CorvoIOExceptionHandler> _logger;
        private readonly IStringLocalizer<CorvoIOExceptionHandler> _localizer;

        public CorvoIOExceptionHandler(ILogger<CorvoIOExceptionHandler> logger)
        {
            _logger = logger;
        }

        public string Handle(Exception exception)
        {
            switch (exception.HResult)
            {
                default:
                    return "";
            }
        }
    }
}