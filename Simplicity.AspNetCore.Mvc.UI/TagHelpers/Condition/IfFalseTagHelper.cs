using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Simplicity.AspNetCore.Mvc.UI.TagHelpers.Condition
{
    public class IfFalseTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var condition = context.Items["Condition"] as bool?;

            if (condition != null && condition.Value)
            {
                output.SuppressOutput();
            }
        }
    }
}