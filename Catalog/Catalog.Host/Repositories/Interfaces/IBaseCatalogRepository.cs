using Catalog.Host.Data;
using Catalog.Host.Repositories.Enums;

namespace Catalog.Host.Repositories.Interfaces
{
    public interface IBaseCatalogRepository<T>
    {
        Task<PaginatedItems<T>> GetByPageAsync(int pageIndex, int pageSize);
        Task<int?> Add(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName);
        Task<int?> Add(string name, EntityType entityType);
        Task<int?> Update(int id, string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName);
        Task<int?> Update(int id, string name, EntityType entityType);
        Task Delete(int id);
        Task<T> GetByIdAsync(int id);
    }
}
