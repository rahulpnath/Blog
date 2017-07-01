using iText.Signatures;
using Microsoft.Azure.KeyVault;
using System.Security.Cryptography;

namespace PdfSign
{
    public class KeyVaultSignature : IExternalSignature
    {
        private KeyVaultClient keyClient;
        private string keyIdentifier;

        public KeyVaultSignature(KeyVaultClient client, string keyIdentifier)
        {
            keyClient = client;
            this.keyIdentifier = keyIdentifier;
        }
        public string GetEncryptionAlgorithm()
        {
            return "RSA";
        }

        public string GetHashAlgorithm()
        {
            return "SHA-256";
        }

        public byte[] Sign(byte[] message)
        {
            var hasher = new SHA256CryptoServiceProvider();
            var digest = hasher.ComputeHash(message);

            return keyClient
                .SignAsync(
                    keyIdentifier,
                    Microsoft.Azure.KeyVault.WebKey.JsonWebKeySignatureAlgorithm.RS256,
                    digest)
                .Result.Result;
        }
    }
}
