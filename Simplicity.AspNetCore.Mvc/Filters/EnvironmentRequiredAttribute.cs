using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Simplicity.AspNetCore.Mvc.Filters;

/// <summary>
/// Enables framework to decide if action should be invoked for current environment.
/// </summary>
public class EnvironmentRequiredAttribute : ActionMethodSelectorAttribute
{
    #region Fields

    /// <summary>
    /// List of allowed form data element names
    /// </summary>
    private readonly string[] _allowedEnvironmentNames;

    #endregion Fields

    #region Ctor

    public EnvironmentRequiredAttribute(params string[] allowedEnvironmentNames)
    {
        _allowedEnvironmentNames = allowedEnvironmentNames;
    }

    #endregion Ctor

    #region Methods

    /// <summary>
    /// Verifies if request is allowed to invoke specified action
    /// </summary>
    /// <param name="routeContext">Route context</param>
    /// <param name="action">Action descriptor</param>
    /// <returns>Result flag</returns>
    public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
    {
        Guard.Against.Null(routeContext, nameof(routeContext));
        Guard.Against.Null(action, nameof(action));

        var environment = routeContext.HttpContext.RequestServices.GetRequiredService<IHostingEnvironment>().EnvironmentName;

        return _allowedEnvironmentNames.Contains(environment);
    }

    #endregion Methods
}