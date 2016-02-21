using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AzureKeyVault
{
    class Program
    {
        private const string applicationId = "AD Application Id";

        private const string applicationSecret = "AD Application secret";

        private const string textToEncrypt = "Text to encrypt";

        static void Main(string[] args)
        {
            Task t = MainAsync(args);
            t.Wait();
        }

        static async Task MainAsync(string[] args)
        {
            var keyClient = new KeyVaultClient(async (authority, resource, scope) =>
            {
                var adCredential = new ClientCredential(applicationId, applicationSecret);
                var authenticationContext = new AuthenticationContext(authority, null);
                return (await authenticationContext.AcquireTokenAsync(resource, adCredential)).AccessToken;
            });

            // Get the key details
            var keyIdentifier = "https://testvaultrahul.vault.azure.net/keys/rahulkey/0f653b06c1d94159bc7090596bbf7784";
            var key = await keyClient.GetKeyAsync(keyIdentifier);
            var publicKey = Convert.ToBase64String(key.Key.N);

            using (var rsa = new RSACryptoServiceProvider())
            {
                var p = new RSAParameters() { Modulus = key.Key.N, Exponent = key.Key.E };
                rsa.ImportParameters(p);
                var byteData = Encoding.Unicode.GetBytes(textToEncrypt);
                
                // Encrypt and Decrypt
                var encryptedText = rsa.Encrypt(byteData, true);
                var decryptedData = await keyClient.DecryptAsync(keyIdentifier, "RSA_OAEP", encryptedText);
                var decryptedText = Encoding.Unicode.GetString(decryptedData.Result);

                // Sign and Verify
                var hasher = new SHA256CryptoServiceProvider();
                var digest = hasher.ComputeHash(byteData);
                var signature = await keyClient.SignAsync(keyIdentifier, "RS256", digest);
                var isVerified = rsa.VerifyHash(digest, "Sha256", signature.Result);
            }
        }
    }
}
