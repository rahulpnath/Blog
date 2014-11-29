namespace RepositoryPattern
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    public class SqlDataStoreStrategy<T> : IDataStoreStrategy<T>
        where T : class, IIdentifiable
    {
        protected readonly SqlDataStoreContext dataContext;

        protected readonly IDbSet<T> dbSet;

        public SqlDataStoreStrategy(IDataStoreContext dataContext)
        {
            // Since this is a specific implementation for Sql it does know about the existence of SqlDataStoreContext
            this.dataContext = dataContext as SqlDataStoreContext;
            this.dbSet = this.dataContext.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            // This might be a costly operation and should be avoided.
            // This can have a lot of objects getting created
            return this.dbSet.ToList();
        }

        T IRepository<T>.Delete(string id)
        {
            var entity = this.dbSet.Find(id);
            return this.dbSet.Remove(entity);
        }

        T IRepository<T>.GetById(string id)
        {
            return this.dbSet.Find(id);
        }

        T IRepository<T>.Insert(T entity)
        {
            return this.dbSet.Add(entity);
        }

        T IRepository<T>.Update(T entity)
        {
            this.dbSet.Attach(entity);
            this.dataContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }
    }
}