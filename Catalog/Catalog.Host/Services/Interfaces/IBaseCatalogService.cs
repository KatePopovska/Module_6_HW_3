using Catalog.Host.Data;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Services.Interfaces
{
    public interface IBaseCatalogService<T>
    {
        Task<int?> Add(T entity);
        Task<int?> Update(T entity);
        Task Delete(int id);
        Task<T> GetById(int id);
    }
}
