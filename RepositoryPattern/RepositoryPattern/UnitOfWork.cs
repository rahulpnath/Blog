namespace RepositoryPattern
{
    using System.Threading.Tasks;

    using Microsoft.Practices.Unity;

    public class UnitOfWork : IUnitOfWork
    {
        private IDataStoreContext dataStoreContext;

        private readonly IUnityContainer container;

        public IRepository<Blog> BlogRepository
        {
            get
            {
                // TODO : Use unity containers to generate the UnitOfwork so that to make surethat
                // datacontext is a single instance in that instance of uow
                return new GenericRepository<Blog>(
                    this.container.Resolve<ICacheStrategy<Blog>>(),
                    new SqlDataStoreStrategy<Blog>(this.dataStoreContext));
            }
        }

        public IArticleRepository ArticleRepository
        {
            get
            {
                return new ArticleRepository(
                    this.container.Resolve<ICacheStrategy<Article>>(),
                    new ArticleSqlDataStoreStrategy(this.dataStoreContext));
            }
        }

        public UnitOfWork(IDataStoreContext dataStoreContext, IUnityContainer container)
        {
            this.dataStoreContext = dataStoreContext;
            this.container = container;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await this.dataStoreContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.dataStoreContext.Dispose();
        }
    }
}