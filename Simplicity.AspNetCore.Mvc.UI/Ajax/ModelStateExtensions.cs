using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Simplicity.AspNetCore.Mvc.UI.Ajax
{
    public static partial class ModelStateExtensions
    {
        /// <summary>
        /// TODO: Extend with validation errors
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static AjaxResponse ToAjaxResponse(this ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
            {
                return new AjaxResponse();
            }

            var errorInfo = new ErrorInfo();

            return new AjaxResponse(errorInfo);
        }
    }
}