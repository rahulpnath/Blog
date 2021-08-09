using System.Threading.Tasks;

namespace AWSLambdaPGPDocker
{
    public interface ICryptoService
    {
        Task<byte[]> Convert(byte[] contents);
    }
}
