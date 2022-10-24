using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace Simplicity.AspNetCore.Mvc.Razor;

/// <summary>
/// Helper methods for rendering views to string
/// </summary>
public sealed class RazorViewToStringRenderer
{
    private readonly IRazorViewEngine _viewEngine;
    private readonly ITempDataProvider _tempDataProvider;
    private readonly IServiceProvider _serviceProvider;

    public RazorViewToStringRenderer(
        IRazorViewEngine viewEngine,
        ITempDataProvider tempDataProvider,
        IServiceProvider serviceProvider)
    {
        _viewEngine = viewEngine;
        _tempDataProvider = tempDataProvider;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Renders given view to a string.
    /// </summary>
    /// <typeparam name="TModel">View model type</typeparam>
    /// <param name="viewName">View name to render</param>
    /// <param name="model">View model</param>
    /// <returns>Rendered string</returns>
    public async Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model)
    {
        var actionContext = GetActionContext();
        var view = FindView(actionContext, viewName);

        using var writer = new StringWriter();

        var viewContext = new ViewContext(
            actionContext,
            view,
            new ViewDataDictionary<TModel>(
                metadataProvider: new EmptyModelMetadataProvider(),
                modelState: new ModelStateDictionary())
            {
                Model = model
            },
            new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
            writer,
            new HtmlHelperOptions());

        await view.RenderAsync(viewContext);

        return writer.ToString();
    }

    /// <summary>
    /// Renders given view to a string.
    /// </summary>
    /// <param name="viewName">View name to render</param>
    /// <returns>Rendered string</returns>
    public Task<string> RenderViewToStringAsync(string viewName) => RenderViewToStringAsync(viewName, new object());

    #region Utils

    private IView FindView(ActionContext actionContext, string viewName)
    {
        var getViewResult = _viewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: true);

        if (getViewResult.Success)
        {
            return getViewResult.View;
        }

        var findViewResult = _viewEngine.FindView(actionContext, viewName, isMainPage: true);

        if (findViewResult.Success)
        {
            return findViewResult.View;
        }        

        var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
        var errorMessage = string.Join(
            Environment.NewLine,
            new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(searchedLocations)); ;

        throw new InvalidOperationException(errorMessage);
    }

    private ActionContext GetActionContext()
    {
        var httpContext = new DefaultHttpContext
        {
            RequestServices = _serviceProvider
        };
        var routeData = new RouteData();
        routeData.Routers.Add(new CustomRouter());
        return new ActionContext(httpContext, routeData, new ActionDescriptor());
    }

    /// <summary>
    /// Custom router to mimic endpoint routing
    /// </summary>
    internal class CustomRouter : IRouter
    {
        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            return null!;
        }

        public Task RouteAsync(RouteContext context)
        {
            return Task.CompletedTask;
        }
    }

    #endregion Utils
}