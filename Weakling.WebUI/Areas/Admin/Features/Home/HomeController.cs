using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weakling.WebUI.Areas.Admin.Features.Home
{

    [Area("Admin")]
    public class HomeController : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {        
            return Content("Index");
        }

    }
}
