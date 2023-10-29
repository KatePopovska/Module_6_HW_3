using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services
{
    public class CatalogTypeService : BaseDataService<ApplicationDbContext>, IBaseCatalogService<CatalogType>
    {
        private readonly IBaseCatalogRepository<CatalogType> _catalogTypeRepository;
        public CatalogTypeService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        IBaseCatalogRepository<CatalogType> catalogTypeRepository)
            : base(dbContextWrapper, logger)
        {
            _catalogTypeRepository = catalogTypeRepository;
        }

        public async Task<int?> Add(CatalogType type)
        {
            return await ExecuteSafeAsync(() => _catalogTypeRepository.Add(type));
        }

        public async Task Delete(int id)
        {
           await ExecuteSafeAsync(() => _catalogTypeRepository.Delete(id));
        }

        public async Task<CatalogType> GetById(int id)
        {
            return await _catalogTypeRepository.GetByIdAsync(id);
        }

        public async Task<int?> Update(CatalogType type)
        {
            return await ExecuteSafeAsync(() => _catalogTypeRepository.Update(type));
        }
    }
}
