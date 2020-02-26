using NReco.PdfGenerator;
using PdfNetCore.Services;
using System.Threading.Tasks;

namespace PdfNetCore.Infrastructure
{
    public class PdfGeneratorService : IPdfGeneratorService
    {
        public IHtmlGenerationService HtmlGenerationService { get; }
        public NRecoConfig Config { get; }

        public PdfGeneratorService(
            IHtmlGenerationService htmlGenerationService, NRecoConfig config)
        {
            HtmlGenerationService = htmlGenerationService;
            Config = config;
        }

        public async Task<byte[]> Generate<T>(T data)
        {
            var htmlContent = await HtmlGenerationService.Generate(data);

            return ToPdf(htmlContent);
        }

        private byte[] ToPdf(string htmlContent)
        {
            var htmlToPdf = new HtmlToPdfConverter();
            htmlToPdf.License.SetLicenseKey(
                Config.UserName,
                Config.License
            );

            return htmlToPdf.GeneratePdf(htmlContent);
        }
    }
}
