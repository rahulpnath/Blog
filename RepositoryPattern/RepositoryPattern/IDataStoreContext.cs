namespace RepositoryPattern
{
    using System;
    using System.Threading.Tasks;

    public interface IDataStoreContext : IDisposable
    {
        Task<int> SaveChangesAsync();
    }
}