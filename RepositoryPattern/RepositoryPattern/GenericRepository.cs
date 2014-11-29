namespace RepositoryPattern
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class GenericRepository<T> : IRepository<T>
        where T : IIdentifiable
    {
        protected ICacheStrategy<T> cacheStrategy;

        protected IDataStoreStrategy<T> dataStoreStrategy;

        public GenericRepository(ICacheStrategy<T> cacheStrategy, IDataStoreStrategy<T> dataStoreStrategy)
        {
            this.cacheStrategy = cacheStrategy;
            this.dataStoreStrategy = dataStoreStrategy;
        }

        public T GetById(string id)
        {
            var item = this.cacheStrategy.Get(id);
            if (item != null)
            {
                return item;
            }

            item = this.dataStoreStrategy.GetById(id);
            this.cacheStrategy.InsertOrUpdate(item);

            return item;
        }

        public IEnumerable<T> GetAll()
        {
            return this.dataStoreStrategy.GetAll();
        }

        public T Delete(string id)
        {
            var entity = this.dataStoreStrategy.Delete(id);
            this.cacheStrategy.Invalidate(id);
            return entity;
        }

        public T Insert(T entity)
        {
            var temp = this.dataStoreStrategy.Insert(entity);
            this.cacheStrategy.InsertOrUpdate(entity);
            return temp;
        }

        public T Update(T entity)
        {
            var temp = this.dataStoreStrategy.Update(entity);
            this.cacheStrategy.InsertOrUpdate(entity);
            return temp;
        }
    }
}