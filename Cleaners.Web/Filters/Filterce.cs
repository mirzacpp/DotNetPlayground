using Cleaners.Web.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cleaners.Web.Filters
{
    public class FilterceFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.Result = new RedirectToRouteResult(HomeRoutes.Index, true);
        }
    }
}