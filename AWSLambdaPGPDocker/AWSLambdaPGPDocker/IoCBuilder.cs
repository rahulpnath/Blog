using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace AWSLambdaPGPDocker
{
    public class IoCBuilder
    {
        public static IServiceProvider BuildProvider()
        {
            var serviceCollection = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            serviceCollection.AddLogging(configure => configure.AddJsonConsole());

            serviceCollection.Configure<PgpSettings>(configuration.GetSection("PgpSettings"));
            serviceCollection.AddTransient<ICryptoService, PgpDockerCryptoService>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
