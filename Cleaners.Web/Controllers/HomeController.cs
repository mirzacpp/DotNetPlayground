using Cleaners.Web.Constants;
using Cleaners.Web.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace Cleaners.Web.Controllers
{
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

        [HttpGet("TestSSE/{number}")]
        public async void TestSSE(int number)
        {
            Response.ContentType = "text/event-stream";
            Response.Headers.Add("Cache-Control", "no-cache");
            var text = "data: This is Server Sent Events testing.";

            await HttpContext.Response.WriteAsync(text);
            await HttpContext.Response.Body.FlushAsync();
            Response.Body.Close();
        }

        [Route("pdf")]
        public IActionResult Pdf()
        {
            var dummySource = new List<string>
            {
                "eb7b13d7","12.3.2016 23:00","Mirza Ćupina","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy",
                "Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy",
                "Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy","Dummy"
            };

            using (var stream = new MemoryStream())
            {
                // Rotate page if needed
                var document = new Document(PageSize.A4);

                PdfWriter.GetInstance(document, stream);

                document.Open();

                PdfPTable table = new PdfPTable(numColumns: 6)
                {
                    WidthPercentage = 100f
                };

                // Fonts can be loaded from application
                Font mainFont = FontFactory.GetFont("Helvetica", size: 18, color: BaseColor.Black, encoding: BaseFont.CP1250, style: Font.NORMAL);
                Font smallFont = FontFactory.GetFont("Helvetica", size: 13, color: BaseColor.Black, encoding: BaseFont.CP1250, style: Font.NORMAL);

                document.Add(new Paragraph("Korekcije na naplatnom mjestu Bijaca", mainFont)
                {
                    SpacingAfter = 5f,
                    Alignment = Element.ALIGN_CENTER
                });

                Chunk linebreak = new Chunk(new LineSeparator(4f, 100f, BaseColor.LightGray, Element.ALIGN_CENTER, -10));
                document.Add(linebreak);

                document.Add(new Paragraph("Period od 21.3.2019 do 23.3.2019", smallFont)
                {
                    SpacingAfter = 20f,
                    Alignment = Element.ALIGN_CENTER
                });

                Font cellFont = FontFactory.GetFont("Helvetica", size: 8, color: new BaseColor(87, 89, 98), encoding: BaseFont.CP1250, style: Font.NORMAL);
                Font cellFontHeader = FontFactory.GetFont("Helvetica", size: 8, color: new BaseColor(87, 89, 98), encoding: BaseFont.CP1250, style: Font.BOLD);

                // Add table headers
                for (int i = 0; i < 6; i++)
                {
                    PdfPCell headerCell = new PdfPCell(new Phrase($"Column {i + 1}", font: cellFontHeader))
                    {
                        //Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER,
                        BorderColor = BaseColor.LightGray,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        Padding = 10f,
                    };

                    table.AddCell(headerCell);
                }

                foreach (var item in dummySource)
                {
                    var dummyCell = new PdfPCell(new Phrase(item, cellFont))
                    {
                        //Border = PdfCell.BOTTOM_BORDER | PdfCell.TOP_BORDER,
                        BorderWidth = .5f,
                        BorderColor = BaseColor.LightGray,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        //FixedHeight = 20f,
                        Padding = 10f
                    };

                    table.AddCell(dummyCell);
                }

                document.Add(table);

                //document.NewPage();
                //document.Add(new Paragraph("This is parahgraph on a new page", font));

                document.Close();

                var bytes = stream.ToArray();

                return File(bytes, "application/pdf", "ok.pdf");
            }
        }
    }
}