using MediatR;
using Microsoft.AspNetCore.Mvc;
using Simplicity.MvcNet6.WebUI.MediatR.Books;
using System.Text.Json;

namespace Simplicity.MvcNet6.WebUI.Features.Localization
{
	public class LocalizationController : Controller
	{
		private readonly IMediator _mediator;

		public LocalizationController(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<IActionResult> Index(BooksSearchViewModel model)
		{
			var results = await _mediator.Send(new BooksQuery
			{
				LangCode = model.LangCode,
				Page = model.Page,
				PageSize = model.PageSize
			});

			return View(results);
		}

		[HttpGet]
		public IActionResult Create() => View(new BookCreateUpdateViewModel());

		[HttpPost]
		public IActionResult Create(BookCreateUpdateViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}			

			return Ok(model);
		}
	}
}