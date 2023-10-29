using Catalog.Host.Data;

namespace Catalog.Host.Repositories.Interfaces
{
    public interface IBaseCatalogRepository<T>
    {
        Task<PaginatedItems<T>> GetByPageAsync(int pageIndex, int pageSize);
        Task<int?> Add(T entity);
        Task<int?> Update(T entity);
        Task Delete(int id);
        Task<T> GetByIdAsync(int id);
    }
}
