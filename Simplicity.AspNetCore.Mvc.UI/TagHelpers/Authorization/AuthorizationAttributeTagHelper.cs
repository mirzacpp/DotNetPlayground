using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Simplicity.AspNetCore.Mvc.UI.TagHelpers.Authorization;

/// <summary>
/// Checks if elements content should be rendered based on user assigned policies
/// <see cref="https://github.com/DamianEdwards/TagHelperPack/blob/master/src/TagHelperPack/AuthzTagHelper.cs"/>
/// </summary>
[HtmlTargetElement("*", Attributes = AspAuthAttributeName)]
[HtmlTargetElement("*", Attributes = AspAuthPolicyAttributeName)]
public class AuthorizationAttributeTagHelper : TagHelper
{
    private const string AspAuthAttributeName = "asp-auth";
    private const string AspAuthPolicyAttributeName = "asp-auth-policy";

    private readonly IAuthorizationService _authorizationService;

    public AuthorizationAttributeTagHelper(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    [HtmlAttributeName(AspAuthAttributeName)]
    public bool RequiresAuthentication { get; set; }

    [HtmlAttributeName(AspAuthPolicyAttributeName)]
    public string RequiredPolicy { get; set; }

    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var requiresAuth = RequiresAuthentication || !string.IsNullOrEmpty(RequiredPolicy);
        var showOutput = false;

        if (context.AllAttributes.ContainsName(AspAuthAttributeName) && !requiresAuth && !ViewContext.HttpContext.User.Identity.IsAuthenticated)
        {
            // Show output in case if attribute is set to false or if user is not authenticated
            showOutput = true;
        }
        else if (!string.IsNullOrEmpty(RequiredPolicy))
        {
            // Cache key per request to preserve result of authorization for this policy
            var cacheKey = AspAuthPolicyAttributeName + "." + RequiredPolicy;

            // First we check if authorization checkup has already been done
            var cachedResult = ViewContext.ViewData[cacheKey];
            bool isAuthorized;
            if (cachedResult != null)
            {
                isAuthorized = (bool)cachedResult;
            }
            else
            {
                var authResult = await _authorizationService.AuthorizeAsync(ViewContext.HttpContext.User, RequiredPolicy);
                isAuthorized = authResult.Succeeded;
                ViewContext.ViewData[cacheKey] = isAuthorized;
            }

            showOutput = isAuthorized;
        }
        else if (requiresAuth && ViewContext.HttpContext.User.Identity.IsAuthenticated)
        {
            // Show output if only users authentication is expected
            showOutput = true;
        }

        if (!showOutput)
        {
            output.SuppressOutput();
        }
    }
}