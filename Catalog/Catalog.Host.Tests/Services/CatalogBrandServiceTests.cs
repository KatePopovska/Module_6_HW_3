
namespace Catalog.Host.Tests.Services
{
    public class CatalogBrandServiceTests
    {
        private readonly IBaseCatalogService<CatalogBrand> _catalogService;

        private readonly Mock<IBaseCatalogRepository<CatalogBrand>> _catalogBrandRepository;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<CatalogService>> _logger;

        private readonly CatalogBrand _testBrand = new CatalogBrand()
        {
            Id = 1,
            Name = "brandName"
        };
        public CatalogBrandServiceTests()
        {
            _catalogBrandRepository = new Mock<IBaseCatalogRepository<CatalogBrand>>();
            _logger = new Mock<ILogger<CatalogService>>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();

            var _dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(_dbContextTransaction.Object);

            _catalogService = new CatalogBrandService(_dbContextWrapper.Object, _logger.Object, _catalogBrandRepository.Object);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            int testResult = 2;
            _catalogBrandRepository.Setup(s => s.Add(It.IsAny<string>(), Repositories.Enums.EntityType.CatalogBrand)).ReturnsAsync(testResult);

            var result = await _catalogService.Add(_testBrand.Name, Repositories.Enums.EntityType.CatalogBrand);

            result.Should().Be(testResult);
        }

        [Fact]
        public async Task AddAsync_Failed()
        {
            int? testResult = null;
            _catalogBrandRepository.Setup(s => s.Add(It.IsAny<string>(), Repositories.Enums.EntityType.CatalogBrand)).ReturnsAsync(testResult);

            var result = await _catalogService.Add(_testBrand.Name, Repositories.Enums.EntityType.CatalogBrand);

            result.Should().Be(testResult);
        }

        [Fact]
        public async Task UpdateAsync_Success()
        {
            int testResult = 3;
            _catalogBrandRepository.Setup(s => s.Update(It.IsAny<int>(), It.IsAny<string>(), Repositories.Enums.EntityType.CatalogBrand)).ReturnsAsync(testResult);

            var result = await _catalogService.Update(_testBrand.Id, _testBrand.Name, Repositories.Enums.EntityType.CatalogBrand);

            result.Should().Be(testResult);
        }

        [Fact]
        public async Task UpdateAsync_Failed()
        {
            int? testResult = null;

            _catalogBrandRepository.Setup(s => s.Update(It.IsAny<int>(), It.IsAny<string>(), Repositories.Enums.EntityType.CatalogBrand)).ReturnsAsync(testResult);

            Func<Task> result = async () => await _catalogService.Update(_testBrand.Id, _testBrand.Name, Repositories.Enums.EntityType.CatalogBrand);

            result.Should().ThrowAsync<Exception>("Something went wrong");
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            int id = 1;

            await _catalogService.Delete(id);

            _catalogBrandRepository.Verify(s => s.Delete(id), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Failed()
        {
            int id = 1;

            _catalogBrandRepository.Setup(s => s.Delete(id)).ThrowsAsync(new KeyNotFoundException());

            Func<Task> result = async () => await _catalogService.Delete(id);

            result.Should().ThrowAsync<KeyNotFoundException>();
        }
    }
}
