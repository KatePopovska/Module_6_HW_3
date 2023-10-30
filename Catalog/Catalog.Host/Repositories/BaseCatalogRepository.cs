using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Enums;
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

        public async Task<int?> Add(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
        {
            var item1 = new CatalogItem
            {
                CatalogBrandId = catalogBrandId,
                CatalogTypeId = catalogTypeId,
                Description = description,
                Name = name,
                PictureFileName = pictureFileName,
                Price = price
            };
            var item = await _dbContext.AddAsync(item1);

            await _dbContext.SaveChangesAsync();

            return item.Entity.Id;
        }

        public async Task<int?> Add(string name, EntityType entityType)
        {
            if(entityType == EntityType.CatalogType)
            {
                var type1 = new CatalogType
                {
                    Name = name,
                };
                var type = await _dbContext.AddAsync(type1);
                return type.Entity.Id;
            }
            else
            {
                var brand1 = new CatalogBrand 
                {
                    Name = name 
                };
                var brand  = await _dbContext.AddAsync(brand1);
                return brand.Entity.Id;
            }
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

        public async Task<int?> Update(int id,string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
        {
            var item = _dbContext.Update(new CatalogItem
            {
                Name = name,
                Description = description,
                Price = price,
                AvailableStock = availableStock,
                CatalogBrandId = catalogBrandId,
                PictureFileName = pictureFileName
            });
            await _dbContext.SaveChangesAsync();
            return item.Entity.Id;
        }

        public async Task<int?> Update(int id, string name, EntityType entityType)
        {
            if (entityType == EntityType.CatalogType)
            {
                var type = _dbContext.Update(new CatalogType { Name = name });
               
                await _dbContext.SaveChangesAsync();
                return type.Entity.Id;
            }
            else
            {
                var brand = _dbContext.Update(new CatalogBrand { Name = name }); 
                await _dbContext.SaveChangesAsync();
                return brand.Entity.Id;
            }
        }
        public abstract Task<PaginatedItems<T>> GetByPageAsync(int pageIndex, int pageSize);
    }
}