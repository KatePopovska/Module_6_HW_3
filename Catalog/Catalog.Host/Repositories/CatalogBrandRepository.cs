using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories
{
    public class CatalogBrandRepository : BaseCatalogRepository<CatalogBrand>
    {
        private readonly ILogger<CatalogBrandRepository> _logger;

        public CatalogBrandRepository(
       IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
       ILogger<CatalogBrandRepository> logger)
       : base(dbContextWrapper)
        {
            _logger = logger;
        }

        public override async Task<PaginatedItems<CatalogBrand>> GetByPageAsync(int pageIndex, int pageSize)
        {
            var totalItems = await _dbContext.CatalogBrands
              .LongCountAsync();

            var itemsOnPage = await _dbContext.CatalogBrands.OrderBy(x => x.Brand)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedItems<CatalogBrand>() { TotalCount = totalItems, Data = itemsOnPage };
        }
    }
}
