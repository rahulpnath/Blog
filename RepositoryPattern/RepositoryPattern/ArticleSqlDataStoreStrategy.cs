namespace RepositoryPattern
{
    using System.Collections.Generic;
    using System.Linq;

    public class ArticleSqlDataStoreStrategy : SqlDataStoreStrategy<Article>, IArticleDataStoreStrategy
    {
        public ArticleSqlDataStoreStrategy(IDataStoreContext dataStoreContext) : base(dataStoreContext)
        {
        }
        public IEnumerable<Article> GetAllArticlesByCategory(string categoryName)
        {
            // In case this is to return a large set of items then you can create a paged response and update the
            // input also to take in the page number and number of articles in one page
            return this.dbSet.Where(a => a.Category == categoryName).ToList();
        }
    }
}