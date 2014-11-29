namespace RepositoryPattern
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;

    public interface IArticleRepository : IRepository<Article>
    {
        IEnumerable<Article> GetAllArticlesByCategory(string categoryName);
    }
}