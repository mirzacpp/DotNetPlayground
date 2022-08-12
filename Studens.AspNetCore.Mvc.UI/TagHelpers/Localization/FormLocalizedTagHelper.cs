namespace Studens.AspNetCore.Mvc.UI.TagHelpers.Localization
{
	/// <summary>
	/// TODO Add service for embedded resources, see https://josef.codes/using-embedded-files-in-dotnet-core/
	/// </summary>
	[HtmlTargetElement(Attributes = "asp-localized")]
	public class FormLocalizedTagHelper : FormTagHelper
	{
		public FormLocalizedTagHelper(IHtmlGenerator generator)
		: base(generator)
		{
		}

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			// Get dropdown .js per css framework type
			var assembly = typeof(FormLocalizedTagHelper).Assembly;
			using var resource = assembly.GetManifestResourceStream("Bootstrap5Dropdown");

			if (resource is not null)
			{
				using var reader = new StreamReader(resource);

				output.PostContent.AppendHtml(@$"<script>{reader.ReadToEnd()}</script>");
			}

			// We will not throw if null since some approaches may not require .js.
		}
	}
}