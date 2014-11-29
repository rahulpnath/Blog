namespace RepositoryPattern
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepository<T> where T : IIdentifiable
    {
        IEnumerable<T> GetAll();
        T Delete(string id);
        T GetById(string id);
        T Insert(T entity);
        T Update(T entity);
    }
}