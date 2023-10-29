
namespace Catalog.Host.Tests.Services
{
    public class CatalogItemServiceTests
    {
        private readonly IBaseCatalogService<CatalogItem> _catalogService;

        private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<CatalogService>> _logger;

        private readonly CatalogItem _testItem = new CatalogItem()
        {
            Id = 33,
            Name = "Name",
            Description = "Description",
            Price = 1000,
            AvailableStock = 100,
            CatalogBrandId = 1,
            CatalogTypeId = 1,
            PictureFileName = "1.png"
        };

        public CatalogItemServiceTests() 
        {
            _catalogItemRepository = new Mock<ICatalogItemRepository>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger= new Mock<ILogger<CatalogService>>();

            var _dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(c => c.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(_dbContextTransaction.Object);

            _catalogService = new CatalogItemService(_dbContextWrapper.Object, _logger.Object, _catalogItemRepository.Object);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            var testResult = 1;

            _catalogItemRepository.Setup(s => s.Add(It.IsAny<CatalogItem>())).ReturnsAsync(testResult);

            var result = await _catalogService.Add(_testItem);

            result.Should().Be(testResult);
        }

        [Fact]
        public async Task AddAsync_Failed()
        {
            int? testResult = null;

            _catalogItemRepository.Setup(s => s.Add(It.IsAny<CatalogItem>())).ReturnsAsync(testResult);

            var result = await _catalogService.Add(_testItem);

            result.Should().Be(testResult);

        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            int id = 3;

            await _catalogService.Delete(id);

            _catalogItemRepository.Verify(c => c.Delete(id), Times.Once());
        }

        [Fact]
        public async Task DeleteAsync_Failed()
        {
            int id = 100;
            _catalogItemRepository.Setup(s => s.Delete(id)).ThrowsAsync(new KeyNotFoundException());

            Func<Task> result = async () => await _catalogService.Delete(id);

            result.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task GetByIdAsync_Success()
        {
            int id = 1;
           _catalogItemRepository.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(_testItem);

             var result = await _catalogService.GetById(id);

            result.Should().NotBeNull();
            result.Should().BeOfType<CatalogItem>();
        }

        [Fact]
        public async Task GetById_Failed()
        {
            int id = 100;
            _catalogItemRepository.Setup(s => s.GetByIdAsync(id)).ThrowsAsync(new KeyNotFoundException());

            Func<Task> result = async () => await _catalogService.GetById(id);

            result.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task UodateAsync_Success()
        {
            var testResult = 1;

           _catalogItemRepository.Setup(s => s.Update(It.IsAny<CatalogItem>())).ReturnsAsync(testResult);

            var result = await _catalogService.Update(_testItem);

            result.Should().NotBeNull();
            result.Should().Be(testResult);
        }
    }
}
