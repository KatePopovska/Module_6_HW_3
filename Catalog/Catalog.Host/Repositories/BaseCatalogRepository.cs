using Catalog.Host.Data;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Repositories
{
    public abstract class BaseCatalogRepository<T> : IBaseCatalogRepository<T>

        where T : class, IEntityWithId
    {
        protected readonly ApplicationDbContext _dbContext;

        protected BaseCatalogRepository(IDbContextWrapper<ApplicationDbContext> dbContextWrapper)
        {
            _dbContext = dbContextWrapper.DbContext;
        }

        public async Task<int?> Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(int id)
        {
            var entityToDelete = await _dbContext.FindAsync<T>(id);
            if (entityToDelete == null)
            {
                throw new KeyNotFoundException("Not Found");
            }

            _dbContext.Set<T>().Remove(entityToDelete);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _dbContext.FindAsync<T>(id);

            if (entity == null)
            {
                throw new KeyNotFoundException("Not Found");
            }

            return entity;
        }

        public async Task<int?> Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public abstract Task<PaginatedItems<T>> GetByPageAsync(int pageIndex, int pageSize);
    }
}