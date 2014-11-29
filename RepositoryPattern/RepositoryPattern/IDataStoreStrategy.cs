namespace RepositoryPattern
{
    public interface IDataStoreStrategy<T> : IRepository<T> where T : IIdentifiable
    {
    }
}