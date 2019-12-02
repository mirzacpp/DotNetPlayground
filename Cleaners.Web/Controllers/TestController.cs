using Ardalis.GuardClauses;
using Cleaners.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cleaners.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class TestController : Controller
    {
        private readonly IFooResolver _fooResolver;

        public TestController(IFooResolver fooResolver)
        {
            _fooResolver = fooResolver;
        }

        public IActionResult CallMe()
        {
            var number = 15;

            Guard.Against.Zero(number, nameof(number));
            //Guard.Against.NegativeNumber(number, nameof(number));

            var instance = _fooResolver.GetInstance(nameof(FooB));

            return Content(instance.Name);
        }

        [HttpGet("{param}", Order = -5)]
        public IActionResult CallMe(int param)
        {
            return Content($"Hello from {nameof(CallMe)} with param {nameof(param)} and value {param}");
        }

        [HttpGet("{ok2}")]
        public IActionResult CallMe(string ok2, string ok)
        {
            return Content($"Hello from {nameof(CallMe)} with param {nameof(ok)} and value {ok}/{ok2}");
        }
    }
}