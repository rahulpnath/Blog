using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmosRest.Domain
{
    public interface IGenericRepository<T>
    {
        Task<IEnumerable<T>> GetAll(string resourceType, Guid resourceId, string modelType);
        Task<T> Get(Guid id, Guid resourceId, string modelType);
        Task Add(T item, Guid resourceId, string modelType);
        Task Update(T item, Guid resourceId, string modelType);
        Task Delete(Guid id, Guid resourceId, string type);
    }
}