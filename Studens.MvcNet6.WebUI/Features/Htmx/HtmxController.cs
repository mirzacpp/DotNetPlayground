using Microsoft.AspNetCore.Mvc;

namespace Studens.MvcNet6.WebUI.Features.Htmx;

public class HtmxController : Controller
{
	[HttpGet]
	public IActionResult Index()
	{
		return View();
	}

	[HttpGet]
	public IActionResult HelloMessage() =>
		Content("Hello from Htmx controller");

	[HttpGet]
	public async Task<IActionResult> HelloMessageDelayed()
	{
		await Task.Delay(2000);

		return Content("Hello from Htmx controller with some delay.");
	}

	[HttpGet]
	public async Task<IActionResult> FetchMeAnError()
	{
		await Task.Delay(2000);

		Response.StatusCode = StatusCodes.Status500InternalServerError;

		return Json(new
		{
			ErrorMessage = "Looks like something went wrong."
		});
	}

	[HttpGet]
	public async Task<IActionResult> Go404()
	{
		return NotFound();
	}

	[HttpGet]
	public async Task<IActionResult> Go400()
	{
		ModelState.AddModelError(string.Empty, "Model state error 1");
		ModelState.AddModelError(string.Empty, "Model state error 2");

		return BadRequest(ModelState);
	}

	[HttpGet]
	public async Task<IActionResult> QuotePoll()
	{
		var random = new Random();

		var colors = new[] {"#FF6633", "#FFB399", "#FF33FF", "#FFFF99", "#00B3E6",
		  "#E6B333", "#3366E6", "#999966", "#99FF99", "#B34D4D",
		  "#80B300", "#809900", "#E6B3B3", "#6680B3", "#66991A",
		  "#FF99E6", "#CCFF1A", "#FF1A66", "#E6331A", "#33FFCC",
		  "#66994D", "#B366CC", "#4D8000", "#B33300", "#CC80CC",
		  "#66664D", "#991AFF", "#E666FF", "#4DB3FF", "#1AB399",
		  "#E666B3", "#33991A", "#CC9999", "#B3B31A", "#00E680",
		  "#4D8066", "#809980", "#E6FF80", "#1AFF33", "#999933",
		  "#FF3380", "#CCCC00", "#66E64D", "#4D80CC", "#9900B3",
		  "#E64D66", "#4DB380", "#FF4D4D", "#99E6E6", "#6666FF"};

		// This should be partial view
		return PartialView("_ColorPoll", colors[random.Next(0, colors.Length - 1)]);
	}

	[HttpPost]
	public async Task<ActionResult> GoForm(FormViewModel model)
	{
		await Task.Delay(1000);

		if (!ModelState.IsValid)
		{
			ModelState.AddModelError(string.Empty, "Form is invalid.");
			Response.StatusCode = StatusCodes.Status400BadRequest;
			//Response.Headers.Add("HX-Push", "https://www.google.com");
			//Response.Headers.Add("HX-Redirect", "https://www.google.com");
			return PartialView("_Form", model);
		}

		return Json(new
		{
			Status = "All good."
		});
	}
}