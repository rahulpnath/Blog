using System.Threading.Tasks;

namespace PdfNetCore.Services
{
    public interface IPdfGeneratorService
    {
        Task<byte[]> Generate<T>(T data);
    }
}
