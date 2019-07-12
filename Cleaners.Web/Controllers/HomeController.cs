using Cleaners.Web.Constants;
using Cleaners.Web.Infrastructure.Files;
using Cleaners.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Cleaners.Web.Controllers
{
    [Authorize]
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ICsvFileService _csvFileService;
        private readonly ICorvoFileProvider _fileProvider;
        private readonly ISelectListProviderService _selectListProviderService;

        public HomeController(ICsvFileService csvFileService, ICorvoFileProvider fileProvider, ISelectListProviderService selectListProviderService)
        {
            _csvFileService = csvFileService ?? throw new ArgumentNullException(nameof(csvFileService));
            _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
            _selectListProviderService = selectListProviderService ?? throw new ArgumentNullException(nameof(selectListProviderService));
        }

        [Route("", Name = HomeRoutes.Index)]
        public IActionResult Index() => View();

        [Route("test")]
        public IActionResult Test()
        {
            return Content(_fileProvider.MapPath("~/cao/sta"));
        }

        [Route("about", Name = HomeRoutes.About)]
        public IActionResult About() => View();
    }
}