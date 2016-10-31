using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.WebKey;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VerifySignatureOffline
{
    class Program
    {
        private static string applicationId = "ApplicationID";
        private static string applicationSecret = "ApplicationSecret";
        static void Main(string[] args)

        {
            var client = new KeyVaultClient(Authenticate);
            GetKeys(client);
            Console.ReadKey();
        }

        private static async Task<string> GetKeys(KeyVaultClient keyVaultClient)
        {
            var keyIdentifier = "keyIdentifier";

            var textToEncrypt = "This is a test message";
            var byteData = Encoding.Unicode.GetBytes(textToEncrypt);
            var hasher = new SHA256CryptoServiceProvider();
            var digest = hasher.ComputeHash(byteData);
            var signedResult = await keyVaultClient.SignAsync(
                keyIdentifier, JsonWebKeySignatureAlgorithm.RS256, digest);

            var isVerified = await keyVaultClient.VerifyAsync(keyIdentifier, "RS256", digest, signedResult.Result);

            var keyResult = await keyVaultClient.GetKeyAsync(keyIdentifier);
            var jsonWebKey = keyResult.Key.ToString();

            var key = JsonConvert.DeserializeObject<JsonWebKey>(jsonWebKey);
            var rsa = new RSACryptoServiceProvider();
            var p = new RSAParameters() { Modulus = key.N, Exponent = key.E };
            rsa.ImportParameters(p);
            
            isVerified = rsa.VerifyHash(digest, "Sha256", signedResult.Result);
            return null;
        }

        private static async Task<string> Authenticate(string authority, string resource, string scope)
        {
            var adCredential = new ClientCredential(applicationId, applicationSecret);
            var authenticationContext = new AuthenticationContext(authority, null);
            return (await authenticationContext.AcquireTokenAsync(resource, adCredential)).AccessToken;
        }
    }
}
