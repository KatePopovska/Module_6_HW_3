using Catalog.Host.Data;
using Catalog.Host.Repositories;
using Catalog.Host.Repositories.Enums;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services
{
    public abstract class BaseCatalogService<T> : BaseDataService<ApplicationDbContext>, IBaseCatalogService<T>

         where T : class, IEntityWithId
    {
        private readonly IBaseCatalogRepository<T> _repository;

        protected BaseCatalogService (IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        IBaseCatalogRepository<T> repository)
        : base(dbContextWrapper, logger)
        {
            _repository = repository;
        }
    
        public async Task<int?> Add(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
        {
            return await ExecuteSafeAsync(() => _repository.Add(name, description, price, availableStock, catalogBrandId, catalogTypeId, pictureFileName));
        }

        public async Task<int?> Add(string name, EntityType entityType)
        {
           return await ExecuteSafeAsync(() => _repository.Add(name, entityType));
        }

        public async Task Delete(int id)
        {
            await ExecuteSafeAsync(() => _repository.Delete(id));
        }

        public async Task<T> GetById(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<int?> Update(int id, string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
        {

            return await ExecuteSafeAsync(() => _repository.Update(id, name, description, price, availableStock, catalogBrandId, catalogTypeId, pictureFileName));
        }
        public async Task<int?> Update(int id, string name, EntityType entityType)
        {
            return await ExecuteSafeAsync(() => _repository.Update(id, name, entityType));
        }
    
    }
}
