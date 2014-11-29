namespace RepositoryPattern
{
    using System.Collections.Generic;

    public class ArticleRepository : GenericRepository<Article>, IArticleRepository
    {
        private IArticleDataStoreStrategy articleDataStoreStrategy;

        public ArticleRepository( ICacheStrategy<Article> cacheStrategy, IArticleDataStoreStrategy articleDataStoreStrategy) : base(cacheStrategy, articleDataStoreStrategy)
        {
            this.articleDataStoreStrategy = articleDataStoreStrategy;
        }
        public IEnumerable<Article> GetAllArticlesByCategory(string categoryName)
        {
            // For sample I am avoiding cache. You could perform cache logic also here
            return this.articleDataStoreStrategy.GetAllArticlesByCategory(categoryName);
        }
    }
}