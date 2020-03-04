using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Corvo.AspNetCore.Mvc.ActionResults
{
    /// <summary>
    /// Enables redirection to given URL for AJAX requests
    /// </summary>
    public class AjaxRedirectResult : IActionResult
    {
        public AjaxRedirectResult(string redirectUrl)
        {
            // Redirect to root page if null passed ...
            RedirectUrl = redirectUrl ?? "/";
        }

        /// <summary>
        /// Use IUrlHelper inside controller to generate URL ?
        /// If not, resolve IUrlHelper here and send routeName/actionName as param
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// These logic is same as for <see cref="ContentResult.ExecuteResultAsync(ActionContext)"/>
        /// We didn't override so we avoid unnecessary parameters
        /// <seealso cref="https://github.com/aspnet/AspNetCore/blob/c565386a3ed135560bc2e9017aa54a950b4e35dd/src/Mvc/Mvc.Core/src/ContentResult.cs"/>
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task ExecuteResultAsync(ActionContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // Since we have to write url only, we can use ContentResult object
            var contentResult = new ContentResult
            {
                Content = RedirectUrl
            };

            var executor = context.HttpContext.RequestServices.GetRequiredService<IActionResultExecutor<ContentResult>>();
            return executor.ExecuteAsync(context, contentResult);
        }
    }
}