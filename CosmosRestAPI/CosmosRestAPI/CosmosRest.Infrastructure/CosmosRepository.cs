using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Cosmos;
using CosmosRest.Domain;
using CosmosRest.Domain.Aggregates;
using CosmosRest.Infrastructure.LOR.Forms.Infrastructure.Cosmos;

namespace CosmosRest.Infrastructure
{
    public class CosmosRepository<T>: IRepository<T> 
    {
        private readonly ICosmosClientFactory _cosmosClientFactory;

        public CosmosRepository(ICosmosClientFactory cosmosClientFactory)
        {
            _cosmosClientFactory = cosmosClientFactory;
        }
        
        public async Task<IEnumerable<T>> GetAll(string resourceType, string resourceId, string modelType)
        {
            var client = _cosmosClientFactory.GetClient(modelType);
            var query = new QueryDefinition($"SELECT * FROM c WHERE c.{resourceType}Id=@resourceId")
                .WithParameter("@resourceId", resourceId);
            var results = client.GetItemQueryIterator<T>(query);
           
            var allItems = new List<T>();
            await foreach (var page in results.AsPages())
            {
                allItems.AddRange(page.Values);
            }

            return allItems;
        }

        public async Task<T> Get(string id, string resourceId, string modelType)
        {
            var client = _cosmosClientFactory.GetClient(modelType);
            return await client.ReadItemAsync<T>(id, new PartitionKey(resourceId));
        }

        public async Task Add(T item, string resourceId, string modelType)
        {
            var container = _cosmosClientFactory.GetClient(modelType);
            await container.CreateItemAsync(
                item,
                new PartitionKey(resourceId),
                GetWriteRequestOptions());
        }

        public async Task Update(T item, string resourceId, string modelType)
        {
            var container = _cosmosClientFactory.GetClient(modelType);
            await container.UpsertItemAsync(
                item,
                new PartitionKey(resourceId),
                GetWriteRequestOptions());
        }

        public async Task Delete(string id, string resourceId, string type)
        {
            var container = _cosmosClientFactory.GetClient(type);
            await container.DeleteItemAsync<T>(
                id,
                new PartitionKey(resourceId),
                GetWriteRequestOptions());
        }
        
        private ItemRequestOptions GetWriteRequestOptions()
        {
            return new ItemRequestOptions() {  };
        }
    }
}