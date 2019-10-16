using System;

namespace Corvo.AspNetCore.Mvc.Middleware.Maintenance
{
    /// <summary>
    /// Enables configuration for <see cref="MaintenanceMiddleware"/>
    /// </summary>
    /// <remarks>
    /// Add list for allowed IP address ?
    /// </remarks>
    public class MaintenanceConfiguration
    {
        private Func<bool> _isEnabled;
        private byte[] _response;

        public MaintenanceConfiguration(Func<bool> isEnabled, byte[] response)
        {
            _isEnabled = isEnabled;
            _response = response;
        }

        public bool IsEnabled => _isEnabled();
        public byte[] Response => _response;

        /// <summary>
        /// Retry-After header value
        /// </summary>
        public int RetryAfterSecondsOffset { get; set; } = 3600;

        public string ContentType { get; set; } = "text/html";
    }
}