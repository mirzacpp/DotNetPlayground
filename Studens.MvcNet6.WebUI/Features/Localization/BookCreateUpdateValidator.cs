using FluentValidation;

namespace Studens.MvcNet6.WebUI.Features.Localization
{
	public class BookCreateUpdateValidator : AbstractValidator<BookCreateUpdateViewModel>
	{
		public BookCreateUpdateValidator()
		{
			RuleForEach(m => m.Title.Translations)
			.ChildRules(element =>
			{
				element
					.RuleFor(c => c.Value)
					.NotEmpty()
					.WithMessage("This culture is required.")
					.When(c => c.LangCode.Equals("en-GB") || c.LangCode.Equals("ar-EG"));
			});
		}
	}
}