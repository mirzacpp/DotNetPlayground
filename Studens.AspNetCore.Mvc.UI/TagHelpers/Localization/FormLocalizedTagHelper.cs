using Studens.Commons.Extensions;

namespace Studens.AspNetCore.Mvc.UI.TagHelpers.Localization
{
	/// <summary>
	/// Indicates that form could contain localized inputs.
	/// This tag helpers appends logic necessary for localization, like attributes, .js etc.
	/// </summary>
	[HtmlTargetElement("form-localized")]
	public class FormLocalizedTagHelper : FormTagHelper
	{
		public FormLocalizedTagHelper(IHtmlGenerator generator)
		: base(generator)
		{
		}

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			// Process by base tag helper
			base.Process(context, output);

			//Change tag name to form
			output.TagName = HtmlTagNames.Form;

			// Get dropdown .js per css framework type
			// Note that currently this will only load resources from current assembly.
			// Also, make sure embedded files are not too large.
			// TODO: If custom files are to be used, maybe we can introduce global generic provider.
			// We can cache embedded files with dictionary since they cannot be changed in runtime.
			var jsScript = JavaScriptResources.GetEmbeddedJavaScript("Bootstrap5Dropdown");

			if (jsScript.IsNotNullOrEmpty())
			{
				// Since we can have multiple localized forms on a single page, we will have to constraint them with form id.
				var formIdAttribute = output.Attributes[TagAttributeNames.Id];
				string formId;

				if (formIdAttribute is not null)
				{
					formId = formIdAttribute.Value.ToString();
				}
				else
				{
					formId = HtmlIdGenerator.GetRandomId(HtmlTagNames.Form);
					// Append new id to form attributes
					output.Attributes.Add(TagAttributeNames.Id, formId);
				}

				output.PostContent.AppendHtml(@$"<script>{jsScript.Replace("__formId__", formId)}</script>");
			}

			// We will not throw if null since some approaches may not require .js.
		}
	}
}