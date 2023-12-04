using Catalog.Host.Data;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories.Interfaces;

public interface ICatalogItemRepository : IBaseCatalogRepository<CatalogItem>
{
    Task<List<CatalogItem>> GetByBrandAsync(string brand);

    Task<List<CatalogItem>> GetByTypeAsync(string type);
}