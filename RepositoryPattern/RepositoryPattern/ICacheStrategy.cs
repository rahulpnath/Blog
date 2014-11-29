namespace RepositoryPattern
{
    using System.Threading.Tasks;

    public interface ICacheStrategy<T> where T : IIdentifiable
    {
        bool InsertOrUpdate(T entity);
        T Get(string id);
        bool Invalidate(string id);
    }
}