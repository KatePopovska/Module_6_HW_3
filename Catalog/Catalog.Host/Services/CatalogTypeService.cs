using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services
{
    public class CatalogTypeService : BaseCatalogService<CatalogType>
    {
        public CatalogTypeService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        IBaseCatalogRepository<CatalogType> catalogTypeRepository)
            : base(dbContextWrapper, logger, catalogTypeRepository)
        {

        }     
    }
}
