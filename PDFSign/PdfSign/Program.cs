using iText.Kernel.Pdf;
using iText.Signatures;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace PdfSign
{
    class Program
    {
        private const string ApplicationId = "APPLICATION ID";
        private const string Secret = "SECRET";
        static void Main(string[] args)
        {
            Task t = MainAsync(args);
            t.Wait();
        }

        static async Task MainAsync(string[] args)
        {
            SignPdfWithLocalCertificate();
            await SignPdfWithExportableCertificateInKeyVault();
            await SignPdfWithNonExportableCertificateInKeyVault();
        }

        private static void SignPdfWithLocalCertificate()
        {
            var certificate = GetCertificateLocal();
            var privateKey = DotNetUtilities.GetKeyPair(certificate.PrivateKey).Private;
            var externalSignature = new PrivateKeySignature(privateKey, "SHA-256");
            SignPdf(certificate, externalSignature, "Local Key.pdf");
        }

        private static async Task SignPdfWithExportableCertificateInKeyVault()
        {
            var client = GetKeyVaultClient();
            var exportableSecretIdentifier = "https://vaultfromcode.vault.azure.net:443/secrets/TestCertificate";
            var certificate = await GetCertificateKeyVault(exportableSecretIdentifier);
            var privateKey = DotNetUtilities.GetKeyPair(certificate.PrivateKey).Private;
            var externalSignature = new PrivateKeySignature(privateKey, "SHA-256");
            SignPdf(certificate, externalSignature, "Exportable Key Vault.pdf");
        }

        private static async Task SignPdfWithNonExportableCertificateInKeyVault()
        {
            var client = GetKeyVaultClient();
            var exportableSecretIdentifier = "https://vaultfromcode.vault.azure.net:443/secrets/TestCertificateNE";
            var certificate = await GetCertificateKeyVault(exportableSecretIdentifier);

            var keyIdentifier = "https://vaultfromcode.vault.azure.net:443/keys/TestCertificateNE/65d27605fdf74eb2a3f807827cd756e1";
            var externalSignature = new KeyVaultSignature(client, keyIdentifier);

            SignPdf(certificate, externalSignature, "Non Exportable Key Vault.pdf");
        }

        private static void SignPdf(X509Certificate2 certificate, IExternalSignature externalSignature, string signedPdfName)
        {
            var bCert = DotNetUtilities.FromX509Certificate(certificate);
            var chain = new Org.BouncyCastle.X509.X509Certificate[] { bCert };

            using (var reader = new PdfReader("Hello World.pdf"))
            {
                using (var stream = new FileStream(signedPdfName, FileMode.OpenOrCreate))
                {
                    var signer = new PdfSigner(reader, stream, false);
                    signer.SignDetached(externalSignature, chain, null, null, null, 0, PdfSigner.CryptoStandard.CMS);
                }
            }
        }

        public static X509Certificate2 GetCertificateLocal()
        {
            var thumbprint = "cc3de775fa314da0e036d75d99b84f94174ffadf";
            var certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);
            var certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
            certStore.Close();

            return certCollection[0];
        }

        public static async Task<X509Certificate2> GetCertificateKeyVault(string secretIdentifier)
        {
            var client = GetKeyVaultClient();
            var secret = await client.GetSecretAsync(secretIdentifier);

            var certSecret = new X509Certificate2(
                Convert.FromBase64String(secret.Value),
                string.Empty,
                X509KeyStorageFlags.Exportable);

            return certSecret;
        }

        private static KeyVaultClient GetKeyVaultClient()
        {
            return new KeyVaultClient(async (authority, resource, scope) =>
            {
                var adCredential = new ClientCredential(ApplicationId, Secret);
                var authenticationContext = new AuthenticationContext(authority, null);
                return (await authenticationContext.AcquireTokenAsync(resource, adCredential)).AccessToken;
            });
        }
    }
}
