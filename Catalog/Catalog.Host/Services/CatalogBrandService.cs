using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services
{
    public class CatalogBrandService : BaseCatalogService<CatalogBrand>
    {
        public CatalogBrandService(IDbContextWrapper<ApplicationDbContext> dbContextWrapper, 
            ILogger<BaseDataService<ApplicationDbContext>> logger,
            IBaseCatalogRepository<CatalogBrand> repository) 
            : base(dbContextWrapper, logger, repository)
        {
        }
    }
}
