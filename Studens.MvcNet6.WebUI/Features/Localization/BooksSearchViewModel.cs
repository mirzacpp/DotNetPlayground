using Microsoft.AspNetCore.Mvc;

namespace Simplicity.MvcNet6.WebUI.Features.Localization
{
	public class BooksSearchViewModel
	{
		public int Page { get; set; } = 1;
		public int PageSize { get; set; } = 10;
		public string LangCode { get; set; } = "it";
	}
}