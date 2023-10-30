using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogItemService : BaseCatalogService<CatalogItem>
{
    public CatalogItemService(IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger, 
        IBaseCatalogRepository<CatalogItem> repository) 
        : base(dbContextWrapper, logger, repository)
    {
    }
}