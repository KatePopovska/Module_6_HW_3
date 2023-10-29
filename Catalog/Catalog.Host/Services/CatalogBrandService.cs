using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services
{
    public class CatalogBrandService : BaseDataService<ApplicationDbContext>, IBaseCatalogService<CatalogBrand>
    {
        private readonly IBaseCatalogRepository<CatalogBrand> _catalogBrandRepository;

        public CatalogBrandService(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> logger,
            IBaseCatalogRepository<CatalogBrand> catalogBrandRepository)
            : base(dbContextWrapper, logger)
        {
            _catalogBrandRepository = catalogBrandRepository;
        }

        public Task<int?> Add(CatalogBrand brand)
        {
            return ExecuteSafeAsync(() => _catalogBrandRepository.Add(brand));
        }

        public async Task Delete(int id)
        {
            await ExecuteSafeAsync(() => _catalogBrandRepository.Delete(id));
        }

        public async Task<CatalogBrand> GetById(int id)
        {
            return await _catalogBrandRepository.GetByIdAsync(id);
        }

        public async Task<int?> Update(CatalogBrand brand)
        {
            return await ExecuteSafeAsync(() => _catalogBrandRepository.Update(brand));
        }
    }
}
