namespace RepositoryPattern
{
    using System;
    using System.Threading.Tasks;

    public interface IUnitOfWork : IDisposable
    {
        IRepository<Blog> BlogRepository { get; }

        IArticleRepository ArticleRepository { get; }

        Task<int> SaveChangesAsync();
    }
}