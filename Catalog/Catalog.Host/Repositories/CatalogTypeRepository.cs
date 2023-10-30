using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories
{
    public sealed class CatalogTypeRepository : BaseCatalogRepository<CatalogType>
    {
        private readonly ILogger<CatalogTypeRepository> _logger;

        public CatalogTypeRepository(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<CatalogTypeRepository> logger)
            : base(dbContextWrapper)
        {
            _logger = logger;
        }

        public override async Task<PaginatedItems<CatalogType>> GetByPageAsync(int pageIndex, int pageSize)
        {
            var totalItems = await _dbContext.CatalogTypes
            .LongCountAsync();

            var itemsOnPage = await _dbContext.CatalogTypes
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedItems<CatalogType>() { TotalCount = totalItems, Data = itemsOnPage };
        }
    }
}
