using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PdfNetCore.Models;
using PdfNetCore.Services;

namespace PdfNetCore.Controllers
{
    [ApiController]
    [Route("pdf")]
    public class PdfController : ControllerBase
    {

        public IPdfGeneratorService PdfGenerationService { get; }
        public IHtmlGenerationService HtmlGenerationService { get; }

        public PdfController(IPdfGeneratorService pdfGenerationService, IHtmlGenerationService htmlGenerationService)
        {
            PdfGenerationService = pdfGenerationService;
            HtmlGenerationService = htmlGenerationService;
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id, [FromQuery]bool? html)
        {
            var model = GetDtoFromStorage(id);

            if (html.GetValueOrDefault())
            {
                var htmlResult = await HtmlGenerationService.Generate(model);
                return new ContentResult() { Content = htmlResult, ContentType = "text/html", StatusCode = 200 };
            }

            var result = await PdfGenerationService.Generate(model);
            return File(result, "application/pdf", $"Quote - {model.Number}.pdf");
        }

        private static QuoteDto GetDtoFromStorage(string id)
        {
            return new QuoteDto()
            {
                Number = id,
                CreatedDate = DateTime.Now,
                 Customer = new CustomerDto()
                 {
                     CustomerName = "Rahul Nath",
                     Address = "100 Unknown Street, Suburb, 1010",
                     Email = "rahul@test.com",
                     Mobile = "099887766"
                 },
                 Discount = 20,
                 SubTotal = 1555,
                 TotalPrice = 1535,
                 Notes = "My Notes",
                 Items = new System.Collections.Generic.List<QuoteItemDto>()
                 {
                     new QuoteItemDto()
                     {
                         Description = "IPhone 11",
                         Quantity = 1,
                         TotalPrice = 1500,
                         UnitPrice = 1500
                     },
                     new QuoteItemDto()
                     {
                         Description = "IPhone 11 Screen Protector",
                         Quantity = 1,
                         TotalPrice = 35,
                         UnitPrice = 35
                     },
                     new QuoteItemDto()
                     {
                         Description = "Discount",
                         Quantity = 1,
                         TotalPrice = 20,
                         UnitPrice = 20
                     }
                 }
            };
        }
    }
}
