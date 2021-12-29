using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Studens.AspNetCore.Mvc.UI.TagHelpers.Condition
{
    public class IfTagHelper : TagHelper
    {
        public bool Condition { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            context.Items["Condition"] = Condition;

            base.Process(context, output);
        }
    }
}