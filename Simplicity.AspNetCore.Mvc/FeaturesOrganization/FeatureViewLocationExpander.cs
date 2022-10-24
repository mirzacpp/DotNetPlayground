using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;

/// <summary>
/// Credits to <see cref="https://github.com/OdeToCode/AddFeatureFolders"/>
/// </summary>
namespace Simplicity.AspNetCore.Mvc.FeaturesOrganization;

public class FeatureViewLocationExpander : IViewLocationExpander
{
    private readonly string _placeholder;

    public FeatureViewLocationExpander(FeatureFolderOptions options)
    {
        _placeholder = options.FeatureNamePlaceholder;
    }

    /// <summary>
    /// See https://stackoverflow.com/questions/36802661/what-is-iviewlocationexpander-populatevalues-for-in-asp-net-core-mvc for more info.
    /// </summary>        
    public void PopulateValues(ViewLocationExpanderContext context) =>
        context.Values[FeaturesConst.ActionDisplayNameKey] = context.ActionContext.ActionDescriptor.DisplayName;

    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
    {
        Guard.Against.Null(context, nameof(context));
        Guard.Against.Null(viewLocations, nameof(viewLocations));

        var controllerDescriptor = context.ActionContext.ActionDescriptor as ControllerActionDescriptor;
        var featureName = controllerDescriptor?.Properties[FeaturesConst.FeaturesKey] as string;

        foreach (var location in viewLocations)
        {
            yield return location.Replace(_placeholder, featureName);
        }
    }
}
