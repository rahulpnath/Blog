using NReco.PdfGenerator;
using PdfNetCore.Services;
using System.Threading.Tasks;

namespace PdfNetCore.Infrastructure
{
    public class PdfGeneratorService : IPdfGeneratorService
    {
        public HtmlToPdfConverter HtmlToPdf { get; }
        public IHtmlGenerationService HtmlGenerationService { get; }

        public PdfGeneratorService(
            HtmlToPdfConverter htmlToPdf,
            IHtmlGenerationService htmlGenerationService)
        {
            HtmlToPdf = htmlToPdf;
            HtmlGenerationService = htmlGenerationService;
        }

        public async Task<byte[]> Generate<T>(T data)
        {
            var htmlContent = await HtmlGenerationService.Generate(data);

            return ToPdf(htmlContent);
        }

        private byte[] ToPdf(string htmlContent)
        {
            return HtmlToPdf.GeneratePdf(htmlContent);
        }
    }
}
