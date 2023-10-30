using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Enums;

namespace Catalog.Host.Services.Interfaces
{
    public interface IBaseCatalogService<T>
    {
        Task<int?> Add(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName);
        Task<int?> Add(string name, EntityType entityType);
        Task<int?> Update(int id, string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName);
        Task<int?> Update(int id, string name, EntityType entityType);
        Task Delete(int id);
        Task<T> GetById(int id);
    }
}
