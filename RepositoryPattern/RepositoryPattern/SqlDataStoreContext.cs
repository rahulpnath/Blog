namespace RepositoryPattern
{
    using System.Data.Entity;
    using System.Diagnostics;

    public class SqlDataStoreContext : DbContext, IDataStoreContext
    {
        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Article> Articles { get; set; }
    }
}