using AutoMapper;
using Catalog.Host.Models.Dtos;

namespace Catalog.Host.Tests.Services
{
    public class CatalogServiceTests
    {
        private readonly ICatalogService _catalogService;

        private readonly Mock<IBaseCatalogRepository<CatalogBrand>> _catalogBrandRepository;
        private readonly Mock<IBaseCatalogRepository<CatalogType>> _catalogTypeRepository;
        private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<CatalogService>> _logger;

        private static CatalogItem _testItem = new CatalogItem()
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


        public CatalogServiceTests()
        {
            _catalogBrandRepository= new Mock<IBaseCatalogRepository<CatalogBrand>>();
            _catalogItemRepository= new Mock<ICatalogItemRepository>();
            _catalogTypeRepository= new Mock<IBaseCatalogRepository<CatalogType>>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _mapper= new Mock<IMapper>();
            _logger = new Mock<ILogger<CatalogService>>();

            var _dbContextTransaction = new Mock<IDbContextTransaction>();

            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(_dbContextTransaction.Object);
            _catalogService = new CatalogService(_dbContextWrapper.Object, _logger.Object,_catalogItemRepository.Object, _catalogBrandRepository.Object,_catalogTypeRepository.Object,_mapper.Object);

        }

        [Theory]
        [InlineData("brand", "GetByBrandAsync")]
        [InlineData("type", "GetByTypeAsync")]
        [InlineData(1, "GetByIdAsync")]       
        public async Task GetByItemOrBrandOrType_Success(object input, string methodName)
        {
            var expextedResultList = new List<CatalogItem> { _testItem };
            var expectedResultSingle = _testItem;

            _catalogItemRepository.Setup(s => s.GetByBrandAsync(It.IsAny<string>())).ReturnsAsync(expextedResultList);
            _catalogItemRepository.Setup(s => s.GetByTypeAsync(It.IsAny<string>())).ReturnsAsync(expextedResultList);
            _catalogItemRepository.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedResultSingle);
            
            object result = null;

            switch(methodName)
            {
                case "GetByBrandAsync":
                    result = await _catalogService.GetByBrandAsync(input as string);
                    break;
                case "GetByTypeAsync":
                    result = await _catalogService.GetByTypeAsync(input as string);
                    break;
                case "GetByIdAsync":
                    result = await _catalogService.GetByIdAsync((int)input);
                    break;
            }

            if (methodName == "GetByBrandAsync" || methodName == "GetByTypeAsync")
            {
                result.Should().NotBeNull();
                result.Should().BeEquivalentTo(expextedResultList);
            }
            else if (methodName == "GetByIdAsync")
            {
                result.Should().NotBeNull();
                result.Should().BeEquivalentTo(expectedResultSingle);
            }
        }

        [Theory]
        [InlineData("brand", "GetByBrandAsync")]
        [InlineData("type", "GetByTypeAsync")]
        [InlineData(1, "GetByIdAsync")]
        public async Task GetByItemOrBrandOrType_Failed(object input, string methodName)
        {
            _catalogItemRepository.Setup(s => s.GetByBrandAsync(It.IsAny<string>())).ThrowsAsync(new KeyNotFoundException(methodName));
            _catalogItemRepository.Setup(s => s.GetByTypeAsync(It.IsAny<string>())).ThrowsAsync(new KeyNotFoundException(methodName));
            _catalogItemRepository.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ThrowsAsync(new KeyNotFoundException(methodName));

            Func <Task> result = null;

            switch (methodName)
            {
                case "GetByBrandAsync":
                    result = async () => await _catalogService.GetByBrandAsync(input as string);
                    break;
                case "GetByTypeAsync":
                    result = async () => await _catalogService.GetByTypeAsync(input as string);
                    break;
                case "GetByIdAsync":
                    result = async () => await _catalogService.GetByIdAsync((int)input);
                    break;
            }

           result.Should().ThrowAsync<KeyNotFoundException>(methodName);
        }

        [Fact]
        public async Task GetCatalogItemsAsync_Success()
        {
            int pageIndex = 1;
            int pageSize = 1;
            var expectedResult = new PaginatedItems<CatalogItem>
            {
                TotalCount = 2,
                Data = new List<CatalogItem> { _testItem}
            };
            var dto = new CatalogItemDto
            {
                Id = _testItem.Id,
                Name = _testItem.Name,
                Description = _testItem.Description,
                Price = _testItem.Price,
            };
            _catalogItemRepository.Setup(s => s.GetByPageAsync(pageIndex, pageSize)).ReturnsAsync(expectedResult);
            _mapper.Setup(mapper => mapper.Map<CatalogItemDto>(It.IsAny<CatalogItem>()))
            .Returns(dto);

            var result = await _catalogService.GetCatalogItemsAsync(pageIndex, pageSize);

            result.Should().NotBeNull();
            result.Count.Should().Be(expectedResult.TotalCount);
            result.PageIndex.Should().Be(pageIndex);
            result.PageSize.Should().Be(pageSize);
            result.Data.Should().NotBeNull().And.HaveCount(expectedResult.Data.Count());
        }

        [Fact]
        public async Task GetCatalogItemsAsync_Failed()
        {
            int pageIndex = 1;
            int pageSize = 1;

            _catalogItemRepository.Setup(s => s.GetByPageAsync(pageIndex, pageSize)).ThrowsAsync(new KeyNotFoundException());

            Func<Task> result = async () => await _catalogService.GetCatalogItemsAsync(pageIndex, pageSize);

            result.Should().NotBeNull();
           result.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]  
        public async Task GetCatalogTypeAsync_Success()
        {
            int pageIndex = 1;
            int pageSize = 1;
            var expectedResult = new PaginatedItems<CatalogType>
            {
                TotalCount = 2,
                Data = new List<CatalogType> { new CatalogType { Name = "test", Id = 2 } }
            };
            var dto = new CatalogTypeDto
            {
                Id = 2,
                Type = "test",
            };
           _catalogTypeRepository.Setup(s => s.GetByPageAsync(pageIndex,pageSize)).ReturnsAsync(expectedResult);
            _mapper.Setup(mapper => mapper.Map<CatalogTypeDto>(It.IsAny<CatalogType>()))
         .Returns(dto);

            var result = await _catalogService.GetCatalogTypeAsync(pageIndex, pageSize);

            result.Should().NotBeNull();
            result.Count.Should().Be(expectedResult.TotalCount);
            result.PageIndex.Should().Be(pageIndex);
            result.PageSize.Should().Be(pageSize);
            result.Data.Should().NotBeNull().And.HaveCount(expectedResult.Data.Count());
        }

        [Fact]
        public async Task GetCatalogTypeAsync_Failed()
        {
            int pageIndex = 1;
            int pageSize = 1;
          
            _catalogTypeRepository.Setup(s => s.GetByPageAsync(pageIndex, pageSize)).ThrowsAsync(new KeyNotFoundException());

            Func<Task> result = async () => await _catalogService.GetCatalogTypeAsync(pageIndex, pageSize);

            result.Should().NotBeNull();
            result.Should().ThrowAsync<KeyNotFoundException>();
        }


        [Fact]
        public async Task GetCatalogBrandAsync_Success()
        {
            int pageIndex = 1;
            int pageSize = 1;
            var expectedResult = new PaginatedItems<CatalogBrand>
            {
                TotalCount = 2,
                Data = new List<CatalogBrand> { new CatalogBrand { Name = "test", Id = 2 } }
            };
            var dto = new CatalogBrandDto
            {
                Brand = "test",
                Id = 2,
            };
            _catalogBrandRepository.Setup(s => s.GetByPageAsync(pageIndex, pageSize)).ReturnsAsync(expectedResult);
            _mapper.Setup(mapper => mapper.Map<CatalogBrandDto>(It.IsAny<CatalogBrand>()))
         .Returns(dto);

            var result = await _catalogService.GetCatalogBrandAsync(pageIndex, pageSize);

            result.Should().NotBeNull();
            result.Count.Should().Be(expectedResult.TotalCount);
            result.PageIndex.Should().Be(pageIndex);
            result.PageSize.Should().Be(pageSize);
            result.Data.Should().NotBeNull().And.HaveCount(expectedResult.Data.Count());
        }

        [Fact]
        public async Task GetCatalogBrandAsync_Failed()
        {
            int pageIndex = 1;
            int pageSize = 1;

            _catalogBrandRepository.Setup(s => s.GetByPageAsync(pageIndex, pageSize)).ThrowsAsync(new KeyNotFoundException());

            Func<Task> result = async () => await _catalogService.GetCatalogBrandAsync(pageIndex, pageSize);

            result.Should().NotBeNull();
            result.Should().ThrowAsync<KeyNotFoundException>();
        }    
    }
}
