using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogItemService : BaseDataService<ApplicationDbContext>, IBaseCatalogService<CatalogItem>
{
    private readonly ICatalogItemRepository _catalogItemRepository;

    public CatalogItemService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogItemRepository catalogItemRepository)
        : base(dbContextWrapper, logger)
    {
        _catalogItemRepository = catalogItemRepository;
    }

    public async Task<int?> Add(CatalogItem item)
    {
        return await ExecuteSafeAsync(() => _catalogItemRepository.Add(item));
    }

    public async Task Delete(int id)
    {
        await ExecuteSafeAsync(() => _catalogItemRepository.Delete(id));
    }

    public async Task<CatalogItem> GetById(int id)
    {
        return await _catalogItemRepository.GetByIdAsync(id);
    }

    public async Task<int?> Update(CatalogItem item)
    {
        return await ExecuteSafeAsync(() => _catalogItemRepository.Update(item));
    }
}