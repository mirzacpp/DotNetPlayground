using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cleaners.Web.Infrastructure.Authentication
{
    /// <summary>
    /// Checks if internal password reset is allowed
    /// </summary>
    public class InternalPasswordResetFilterAttribute : ActionFilterAttribute
    {
        private readonly IdentityConfig _identityConfig;

        public InternalPasswordResetFilterAttribute(IdentityConfig identityConfig)
        {
            _identityConfig = identityConfig;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Guard.Against.Null(context, nameof(context));

            // Set result as Forrbiden if internal password reset is not allowed
            if (!_identityConfig.DefaultOptions.InternalPasswordResetEnabled)
            {
                context.Result = new StatusCodeResult(403);
            }
        }
    }
}