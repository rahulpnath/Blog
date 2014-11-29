namespace RepositoryPattern
{
    public interface IArticleDataStoreStrategy : IDataStoreStrategy<Article>, IArticleRepository
    {
    }
}