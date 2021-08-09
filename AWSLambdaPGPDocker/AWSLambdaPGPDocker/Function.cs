using Amazon;
using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3.Util;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSLambdaPGPDocker
{
    public class Function
    {
        private ICryptoService cryptoService;
        IAmazonS3 S3Client { get; set; }

        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Function()
        {
            Console.WriteLine("Logging to Console set " + AWSConfigs.LoggingConfig.LogTo);
            AWSConfigs.LoggingConfig.LogTo = LoggingOptions.Console;

            S3Client = new AmazonS3Client();
            ;

            var ioc = IoCBuilder.BuildProvider();
            this.cryptoService = ioc.GetService<ICryptoService>();
        }

        /// <summary>
        /// Constructs an instance with a preconfigured S3 client. This can be used for testing the outside of the Lambda environment.
        /// </summary>
        /// <param name="s3Client"></param>
        public Function(IAmazonS3 s3Client)
        {
            this.S3Client = s3Client;
        }

        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an S3 event object and can be used 
        /// to respond to S3 notifications.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(S3Event evnt, ILambdaContext context)
        {
            var s3Event = evnt.Records?[0].S3;
            if (s3Event == null) return;

            var bucketName = s3Event.Bucket.Name;
            var fileName = s3Event.Object.Key;

            try
            {
                Console.WriteLine($"New File {bucketName} {fileName}");

                var byteContent = await ReadFileContent(bucketName, fileName);
                var encryptedFileContents = await cryptoService.Convert(byteContent);
                await SaveEncryptedFile($"{bucketName}-encrypted", fileName, encryptedFileContents);

                Console.WriteLine("File Successfully encrypted and saved");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                throw;
            }
        }

        private async Task SaveEncryptedFile(
            string bucketName, string fileName, byte[] encryptedFileContents)
        {
            using (Stream stream = new MemoryStream(encryptedFileContents))
            {
                var putObject = new PutObjectRequest()
                {
                    BucketName = bucketName,
                    InputStream = stream,
                    Key = fileName
                };

                await S3Client.PutObjectAsync(putObject);
            }
        }

        private async Task<byte[]> ReadFileContent(string bucketName, string fileName)
        {
            byte[] byteContent;
            var response = await S3Client.GetObjectAsync(bucketName, fileName);

            using (var ms = new MemoryStream())
            {
                await response.ResponseStream.CopyToAsync(ms);
                byteContent = ms.ToArray();
            }

            return byteContent;
        }
    }
}