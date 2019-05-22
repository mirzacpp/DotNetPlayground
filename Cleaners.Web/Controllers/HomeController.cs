using Cleaners.Web.Constants;
using Cleaners.Web.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace Cleaners.Web.Controllers
{
    public class Dummy
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Age { get; set; }
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

        [Route("pdf")]
        public IActionResult Pdf()
        {
            var list = new List<Dummy>
            {
                new Dummy { FirstName = "Vladimir", LastName = "Vladmir", Age = "20"},
                new Dummy { FirstName = "Vladimir", LastName = "Vladmir", Age = "20"},
                new Dummy { FirstName = "Vladimir", LastName = "Vladmir", Age = "20"},
                new Dummy { FirstName = "Vladimir", LastName = "Vladmir", Age = "20"},
                new Dummy { FirstName = "Vladimir", LastName = "Vladmir", Age = "20"},
                new Dummy { FirstName = "Vladimir", LastName = "Vladmir", Age = "20"},
                new Dummy { FirstName = "Vladimir", LastName = "Vladmir", Age = "20"},
                new Dummy { FirstName = "Vladimir", LastName = "Vladmir", Age = "20"},
                new Dummy { FirstName = "Vladimir", LastName = "Vladmir", Age = "20"},
                new Dummy { FirstName = "Vladimir", LastName = "Vladmir", Age = "20"},
                new Dummy { FirstName = "Vladimir", LastName = "Vladmir", Age = "20"},
                new Dummy { FirstName = "Vladimir", LastName = "Vladmir", Age = "20"},
                new Dummy { FirstName = "Vladimir", LastName = "Vladmir", Age = "20"},
                new Dummy { FirstName = "Vladimir", LastName = "Vladmir", Age = "20"},
                new Dummy { FirstName = "Vladimir", LastName = "Vladmir", Age = "20"},
                new Dummy { FirstName = "Vladimir", LastName = "Vladmir", Age = "20"},
                new Dummy { FirstName = "Vladimir", LastName = "Vladmir", Age = "20"},
                new Dummy { FirstName = "Vladimir", LastName = "Vladmir", Age = "20"},
                new Dummy { FirstName = "Vladimir", LastName = "Vladmir", Age = "20"},
                new Dummy { FirstName = "Vladimir", LastName = "Vladmir", Age = "20"},
                new Dummy { FirstName = "Vladimir", LastName = "Vladmir", Age = "20"},
                new Dummy { FirstName = "Vladimir", LastName = "Vladmir", Age = "20"}
            };

            using (var stream = new MemoryStream())
            {
                // Rotate page if needed
                var document = new Document(PageSize.A4/*.Rotate()*/);

                PdfWriter.GetInstance(document, stream);

                document.Open();

                PdfPTable table = new PdfPTable(numColumns: 6)
                {
                    WidthPercentage = 100f
                };

                // Fonts can be loaded from application
                PdfPCell cell = new PdfPCell(new Phrase("This is a title row", font: FontFactory.GetFont(FontFactory.HELVETICA)));
                cell.Colspan = 6;                
                cell.BackgroundColor = BaseColor.Red;
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
                table.AddCell("Dasa");
                table.AddCell("Dasa2");
                table.AddCell("Dasa3");
                table.AddCell("Dasa4");
                table.AddCell("Dasa5");
                table.AddCell("Dasa6");

                document.Add(table);

                document.Close();

                var bytes = stream.ToArray();

                return File(bytes, "application/pdf", "ok.pdf");

                //PdfTable table = new PdfTable(3);
            }
        }
    }
}