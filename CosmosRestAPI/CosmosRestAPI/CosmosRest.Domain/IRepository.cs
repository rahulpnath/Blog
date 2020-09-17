using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CosmosRest.Domain.Aggregates;

namespace CosmosRest.Domain
{
    public interface IRepository<T> 
    {
        Task<IEnumerable<T>> GetAll(string resourceType, string resourceId, string modelType);
        Task<T> Get(string id, string resourceId, string modelType);
        Task Add(T item, string resourceId, string modelType);
        Task Update(T item, string resourceId, string modelType);
        Task Delete(string id, string resourceId, string type);
    }
}