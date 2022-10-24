namespace Simplicity.AspNetCore.Mvc.UI.TagHelpers.Localization
{
	public interface IInputControlGenerator
	{
		TagBuilder Generate(LocalizationControlContext context);
	}
}