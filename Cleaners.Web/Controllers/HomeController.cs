using Cleaners.Web.Constants;
using Cleaners.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Cleaners.Web.Controllers
{
    public class Dummy
    {
        public int Prop1 { get; set; }
        public string Prop2 { get; set; }
    }

    [Authorize]
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ICsvFileService _csvFileService;

        public HomeController(ICsvFileService csvFileService)
        {
            _csvFileService = csvFileService ?? throw new ArgumentNullException(nameof(csvFileService));
        }

        [Route("", Name = HomeRoutes.Index)]
        public IActionResult Index() => View();

        [Route("about", Name = HomeRoutes.About)]
        public IActionResult About() => View();

        [Route("test")]
        public IActionResult Test()
        {
            var data = new List<Dummy>
            {
                new Dummy{ Prop1 = 1, Prop2 = "Ok vlada"},
                new Dummy{ Prop1 = 1, Prop2 = "Ok vlada"},
                new Dummy{ Prop1 = 1, Prop2 = "Ok vlada"},
                new Dummy{ Prop1 = 1, Prop2 = "Ok vlada"},
                new Dummy{ Prop1 = 1, Prop2 = "Ok vlada"},
                new Dummy{ Prop1 = 1, Prop2 = "Ok vlada"},
                new Dummy{ Prop1 = 1, Prop2 = "Ok vlada"},
                new Dummy{ Prop1 = 1, Prop2 = "Ok vlada"},
                new Dummy{ Prop1 = 1, Prop2 = "Ok vlada"},
                new Dummy{ Prop1 = 1, Prop2 = "Ok vlada"},
                new Dummy{ Prop1 = 1, Prop2 = "Ok vlada"},
                new Dummy{ Prop1 = 1, Prop2 = "Ok vlada"},
                new Dummy{ Prop1 = 1, Prop2 = "Ok vlada"},
                new Dummy{ Prop1 = 1, Prop2 = "Ok vlada"},
                new Dummy{ Prop1 = 1, Prop2 = "Ok vlada"},
                new Dummy{ Prop1 = 1, Prop2 = "Ok vlada"},
                new Dummy{ Prop1 = 1, Prop2 = "Ok vlada"}
            };

            var bytes = _csvFileService.GenerateCsv(data);

            return File(bytes, "text/csv", "csv-file.csv");            
        }
    }
}