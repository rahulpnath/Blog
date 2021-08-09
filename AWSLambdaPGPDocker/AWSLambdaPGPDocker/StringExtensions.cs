using System;
using System.Text;

namespace AWSLambdaPGPDocker
{
    public static class StringExtensions
    {
        public static string Base64Decode(this string encodedString)
        {
            byte[] data = Convert.FromBase64String(encodedString);
            return Encoding.UTF8.GetString(data);
        }
    }
}
