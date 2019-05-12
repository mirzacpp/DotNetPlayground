using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace Cleaners.Web.Infrastructure.Authentication
{
    /// <summary>
    /// Checks if internal password reset is allowed
    /// </summary>
    public class InternalPasswordResetFilter : ActionFilterAttribute
    {
        private readonly IdentityConfig _identityConfig;

        public InternalPasswordResetFilter(IdentityConfig identityConfig)
        {
            _identityConfig = identityConfig ?? throw new ArgumentNullException(nameof(identityConfig));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Set result as Forrbiden if internal password reset is not allowed
            if (!_identityConfig.DefaultOptions.InternalPasswordResetEnabled)
            {
                context.Result = new StatusCodeResult(403);
            }

            base.OnActionExecuting(context);
        }
    }
}