using System.ComponentModel.DataAnnotations;

namespace Studens.MvcNet6.WebUI.Features.Htmx
{
	public class FormViewModel
	{
		[Required(ErrorMessage = "Upps, looks like you forgot your first name.")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Upps, looks like you forgot your last name.")]
		public string LastName { get; set; }
	}
}