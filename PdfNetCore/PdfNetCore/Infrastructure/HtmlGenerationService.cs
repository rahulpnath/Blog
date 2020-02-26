using PdfNetCore.Services;
using RazorLight;
using System.Threading.Tasks;

namespace PdfNetCore.Infrastructure
{
    public class HtmlGenerationService : IHtmlGenerationService
    {
        private readonly IRazorLightEngine _razorLightEngine;

        public HtmlGenerationService(IRazorLightEngine razorLightEngine)
        {
            _razorLightEngine = razorLightEngine;
        }
        public async Task<string> Generate<T>(T data)
        {
            var template = typeof(T).Name.Replace("Dto", "", System.StringComparison.InvariantCultureIgnoreCase);
            return await _razorLightEngine.CompileRenderAsync($"{template}/{template}.cshtml", data);
        }
    }
}
