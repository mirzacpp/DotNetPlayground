namespace Simplicity.AspNetCore.Mvc.UI.TagHelpers
{
	public static partial class TagBuilderExtensions
	{
		public static TagBuilder AsTextInput(this TagBuilder tagBuilder) =>
		tagBuilder.AsInput(TagAttributeValues.Text);

		public static TagBuilder AsInput(this TagBuilder tagBuilder, string type)
		{
			tagBuilder.Attributes.Add(TagAttributeNames.Type, type);
			return tagBuilder;
		}

		public static TagBuilder WithId(this TagBuilder tagBuilder, string id)
		{
			tagBuilder.Attributes.Add(TagAttributeNames.Id, id);
			return tagBuilder;
		}

		public static TagBuilder WithName(this TagBuilder tagBuilder, string name)
		{
			tagBuilder.Attributes.Add(TagAttributeNames.Name, name);
			return tagBuilder;
		}

		public static TagBuilder WithClass(this TagBuilder tagBuilder, string cssClass)
		{
			tagBuilder.AddCssClass(cssClass);
			return tagBuilder;
		}

		public static TagBuilder WithValue(this TagBuilder tagBuilder, string value)
		{
			tagBuilder.Attributes.Add("value", value);
			return tagBuilder;
		}

		public static TagBuilder WithAttribute(this TagBuilder tagBuilder, string name, string value)
		{
			tagBuilder.Attributes.Add(name, value);
			return tagBuilder;
		}
	}
}