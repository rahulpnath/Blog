using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace CosmosEmulator.ArmTemplate
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var containers = await GetContainersFromTemplate(@"deploy\azuredeploy.parameters.json");

            var cosmosConfig = GetCosmosConfig();
            var cosmosClient = new CosmosClient(
                cosmosConfig.EndpointUrl,
                cosmosConfig.PrimaryKey,
                new CosmosClientOptions());

            await cosmosClient.CreateDatabaseIfNotExistsAsync(cosmosConfig.DatabaseName);

            var database = cosmosClient.GetDatabase(cosmosConfig.DatabaseName);
            foreach (var container in containers)
                await database.CreateContainerIfNotExistsAsync(container.Name, container.PartitionKey);
        }

        private static CosmosConfig GetCosmosConfig()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var cosmosConfig = config.GetSection("Cosmos").Get<CosmosConfig>();
            return cosmosConfig;
        }

        private static async Task<List<ContainerTemplate>> GetContainersFromTemplate(string templateFilePath)
        {
            var cosmosParameterFile = await File.ReadAllTextAsync(templateFilePath);
            var jsonObject = JObject.Parse(cosmosParameterFile);
            var containers = jsonObject["parameters"]["containers"]["value"]
                .ToObject<List<ContainerTemplate>>();
            return containers;
        }
    }
}