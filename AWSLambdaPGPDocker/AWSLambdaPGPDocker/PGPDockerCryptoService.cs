using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSLambdaPGPDocker
{
    public class PgpDockerCryptoService : ICryptoService
    {
        private const string Root = "/tmp";
        private readonly string RecipientPublicKeyFile = $"{Root}/RecipientPublicKey.pgp";
        private readonly string SenderPrivateKeyFile = $"{Root}/PrivateKey.pgp";
        private readonly string OriginalFile = $"{Root}/Original.csv";
        private readonly string OutputFileName = $"{Root}/PGP.csv";
        private readonly ILogger<PgpDockerCryptoService> logger;

        public PgpSettings PgpSettings { get; }

        public PgpDockerCryptoService(
            IOptions<PgpSettings> pgpSettings,
            ILogger<PgpDockerCryptoService> logger)
        {
            PgpSettings = pgpSettings.Value;
            this.logger = logger;
        }

        public async Task<byte[]> Convert(byte[] fileContent)
        {
            await SaveCertsToDisk();
            await ImportCertificatesIntoPgp();
            return await PgpEncryptAndSign(fileContent);
        }

        private async Task<byte[]> PgpEncryptAndSign(byte[] fileContent)
        {
            logger.LogInformation("Starting PGP EncryptAndSign File Contents");
            var senderUserId = PgpSettings.SenderUserId;
            var recipientUserId = PgpSettings.RecipientUserId;
            var passphrase = PgpSettings.SenderPassphrase;

            await System.IO.File.WriteAllBytesAsync(OriginalFile, fileContent);
            await RunGpgExternalProcess(
                $"--no-tty --trust-model always --batch --passphrase {passphrase} --recipient {recipientUserId} --encrypt --sign --local-user {senderUserId} --armor -o {OutputFileName} {OriginalFile}");
            return await System.IO.File.ReadAllBytesAsync(OutputFileName);
        }

        private async Task ImportCertificatesIntoPgp()
        {
            logger.LogInformation("Importing Certificates into pgp keyring");
            string passphrase = PgpSettings.SenderPassphrase;

            await RunGpgExternalProcess( // Import Private Key into Key Ring
                $"--passphrase \"{passphrase}\" --import {SenderPrivateKeyFile}");
            await RunGpgExternalProcess( // Import Public Key into Key Ring
                $"--import {RecipientPublicKeyFile}"); 
            logger.LogInformation("Imported Certificates into pgp keyring");
        }

        private async Task RunGpgExternalProcess(string arguments)
        {
            using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
            {
                string result = "";
                proc.StartInfo.FileName = "gpg";
                proc.StartInfo.Arguments = $"--homedir \"{Root}/\" {arguments}";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();

                result += await proc.StandardOutput.ReadToEndAsync();
                result += await proc.StandardError.ReadToEndAsync();
                proc.WaitForExit();
                logger.LogInformation($"GPG External Process - {result}");
            }
        }

        private async Task SaveCertsToDisk()
        {
            logger.LogInformation("Saving GPG Certificates Locally");
            string recipientPublicKey = PgpSettings.RecipientPublicKey.Base64Decode();
            string senderPrivateKey = PgpSettings.SenderPrivateKey.Base64Decode();
            await System.IO.File.WriteAllTextAsync(RecipientPublicKeyFile, recipientPublicKey);
            await System.IO.File.WriteAllTextAsync(SenderPrivateKeyFile, senderPrivateKey);
            logger.LogInformation("Saved GPG Certificates Locally");
        }
    }

    public class PgpSettings
    {
        public string SenderUserId { get; set; }
        public string RecipientUserId { get; set; }
        public string RecipientPublicKey { get; set; }
        public string SenderPrivateKey { get; set; }
        public string SenderPassphrase { get; set; }
    }
}