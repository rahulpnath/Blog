using System.ComponentModel;
using Azure.Cosmos;

namespace CosmosRest.Infrastructure
{
    using Microsoft.Azure.Cosmos;

    public interface ICosmosClientFactory
    {
        CosmosContainer GetClient(string collectionName);
    }

    public class CosmosClientFactory : ICosmosClientFactory
    {
        private readonly CosmosClient _client;
        private readonly string _databaseName;

        public CosmosClientFactory(CosmosConfiguration configuration)
        {
            _client = new CosmosClient(
                configuration.EndpointUrl,
                configuration.PrimaryKey
            );
            _databaseName = configuration.DatabaseName;
        }

        public CosmosContainer GetClient(string collectionName)
        {
            return _client.GetContainer(_databaseName, collectionName);
        }
    }
}