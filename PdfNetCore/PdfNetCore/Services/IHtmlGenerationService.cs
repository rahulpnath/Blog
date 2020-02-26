using System.Threading.Tasks;

namespace PdfNetCore.Services
{
    public interface IHtmlGenerationService
    {
        Task<string> Generate<T>(T data);
    }
}
