using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public sealed class CatalogItemRepository : BaseCatalogRepository<CatalogItem>, ICatalogItemRepository
{
    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogItemRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogItemRepository> logger)
        : base(dbContextWrapper)
    {
        _logger = logger;
    }

    public override async Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize)
    {
        var totalItems = await _dbContext.CatalogItems
            .LongCountAsync();

        var itemsOnPage = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<List<CatalogItem>> GetByBrandAsync(string brand)
    {
        var items = await _dbContext.CatalogItems.Include(x => x.CatalogBrand)
            .Where(x => x.CatalogBrand.Name == brand)
            .OrderBy(x => x.CatalogBrand.Name)
            .ToListAsync();

        if (items.Count == 0)
        {
            throw new KeyNotFoundException();
        }

        return items;
    }

    public async Task<List<CatalogItem>> GetByTypeAsync(string type)
    {
        var items = await _dbContext.CatalogItems.Include(x => x.CatalogType)
            .Where(x => x.CatalogType.Name == type)
            .OrderBy(x => x.CatalogType.Name)
            .ToListAsync();

        if (items.Count == 0)
        {
            throw new KeyNotFoundException();
        }

        return items;
    }
}
