using FluentValidation;

namespace Simplicity.MvcNet6.WebUI.Features.Localization
{
	public class BookCreateUpdateValidator : AbstractValidator<BookCreateUpdateViewModel>
	{
		public BookCreateUpdateValidator()
		{
			//ValidatorOptions.Global.DisplayNameResolver = (type, memeber, expression) =>
			//{
			//	if (memeber != null)
			//	{
			//		return "Title";
			//	}

			//	return null;
			//};

			//RuleFor(m => m.Title)
			//.ChildRules(val =>
			//{
			//	val.RuleForEach(x => x.Translations)
			//	.ChildRules(element =>
			//	{
			//		element
			//			.RuleFor(c => c.Value)
			//			.NotEmpty()
			//			.WithMessage(entry => $"Input is required for '{entry.LangCode}' culture.")
			//			.OverridePropertyName(nameof(BookCreateUpdateViewModel.Title))
			//			.When(c => c.LangCode.Equals("en-GB") || c.LangCode.Equals("ar-EG"));
			//	});
			//});

			RuleForEach(m => m.Title.Translations)
			.ChildRules(action =>
			{
				action.RuleFor(c => c.Value)
				.Must((model, value, ctx) =>
				{
					if (string.IsNullOrEmpty(value))
					{
						ctx.AddFailure("Title", $"Input is required for '{model.LangCode}' culture.");
						return false;
					}

					return true;
				})
				.When(c => c.LangCode.Equals("en-GB") || c.LangCode.Equals("ar-EG"));
				

				//action
				//	.RuleFor(c => c.Value)
				//	.NotEmpty()
				//	.WithMessage("This culture is required.")
				//	.WithName(nameof(BookCreateUpdateViewModel.Title))
				//	.When(c => c.LangCode.Equals("en-GB") || c.LangCode.Equals("ar-EG"));
			});
		}
	}
}