
namespace Catalog.Host.Tests.Services
{
    public class CatalogTypeServiceTests
    {
        private readonly  IBaseCatalogService<CatalogType> _catalogService;

        private readonly Mock<IBaseCatalogRepository<CatalogType>> _catalogTypeRepository;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<CatalogService>> _logger;

        private readonly CatalogType _testType = new CatalogType()
        {
            Id = 1,
            Type = "typeName"
        };

        public CatalogTypeServiceTests()
        {
            _catalogTypeRepository = new Mock<IBaseCatalogRepository<CatalogType>>();
            _logger = new Mock<ILogger<CatalogService>>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();

            var _dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(_dbContextTransaction.Object);

            _catalogService = new CatalogTypeService(_dbContextWrapper.Object, _logger.Object, _catalogTypeRepository.Object);
        }

        [Fact]
        public async Task AddAsync_Success()
        {            
            int testResult = 2;
            _catalogTypeRepository.Setup(s => s.Add(It.IsAny<CatalogType>())).ReturnsAsync(testResult);

            var result = await _catalogService.Add(_testType);

            result.Should().Be(testResult);
        }

        [Fact]
        public async Task AddAsync_Failed()
        {
            int? testResult = null;
            _catalogTypeRepository.Setup(s => s.Add(It.IsAny<CatalogType>())).ReturnsAsync(testResult);

            var result = await _catalogService.Add(_testType);

            result.Should().Be(testResult);
        }

        [Fact]
        public async Task UpdateAsync_Success()
        {
            int testResult = 3;
             _catalogTypeRepository.Setup(s => s.Update(It.IsAny<CatalogType>())).ReturnsAsync(testResult);

            var result = await _catalogService.Update(_testType);

            result.Should().Be(testResult);
        }

        [Fact]
        public async Task UpdateAsync_Failed()
        {
            int? testResult = null;

            _catalogTypeRepository.Setup(s => s.Update(_testType)).ThrowsAsync(new Exception("Something went wrong"));

            Func<Task> result = async () => await _catalogService.Update(_testType);

            result.Should().ThrowAsync<Exception>("Something went wrong");
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            int id = 1;

            await _catalogService.Delete(id);

            _catalogTypeRepository.Verify(s => s.Delete(id), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Failed ()
        {
            int id = 1;

            _catalogTypeRepository.Setup(s => s.Delete(id)).ThrowsAsync(new KeyNotFoundException());

            Func<Task> result = async () => await _catalogService.Delete(id);

            result.Should().ThrowAsync<KeyNotFoundException>();
        }
    }
}
