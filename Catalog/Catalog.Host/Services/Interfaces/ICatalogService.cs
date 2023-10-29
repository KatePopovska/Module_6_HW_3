using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogService
{
    Task<PaginatedItemsResponse<CatalogItemDto>> GetCatalogItemsAsync(int pageSize, int pageIndex);
    Task<CatalogItem> GetByIdAsync(int id);

    Task<List<CatalogItem>> GetByBrandAsync(string brand);

    Task<List<CatalogItem>> GetByTypeAsync(string type);

    Task<PaginatedItemsResponse<CatalogBrandDto>> GetCatalogBrandAsync(int pageIndex, int pageSize);

    Task<PaginatedItemsResponse<CatalogTypeDto>> GetCatalogTypeAsync(int pageIndex, int pageSize);
}