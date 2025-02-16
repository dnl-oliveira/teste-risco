using Categorize.Application.Interfaces;
using Categorize.Application.Services;
using Categorize.Domain.Interfaces;
using Moq;
using Categorize.Domain.Models;

namespace Categorize.Tests
{
    public class CategorizerTests
    {
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly ICategorizerService _categorizerService;

        public CategorizerTests()
        {
            _categoryServiceMock = new Mock<ICategoryService>();
            _categorizerService = new CategorizerService(_categoryServiceMock.Object);
        }

        [Fact]
        public async Task Categorize_ShouldReturnExpiredAsync()
        {
            // Arrange
            var tradeMock = new Mock<ITrade>();
            tradeMock.Setup(t => t.NextPaymentDate).Returns(new DateTime(2020, 11, 01));
            var referenceDate = new DateTime(2021, 01, 01);

            var categories = new List<Category>
            {
                new Category { Name = "EXPIRED", CompiledRule = (trade, date) => trade.NextPaymentDate < date.AddDays(-30) }
            };

            _categoryServiceMock.Setup(cs => cs.GetCategoriesAsync(referenceDate)).ReturnsAsync(categories);

            // Act
            var result = await _categorizerService.Categorize(tradeMock.Object, referenceDate);

            // Assert
            Assert.Equal("EXPIRED", result);
        }

        [Fact]
        public async Task Categorize_ShouldReturnHighRisk()
        {
            // Arrange
            var tradeMock = new Mock<ITrade>();
            tradeMock.Setup(t => t.Value).Returns(2000000);
            tradeMock.Setup(t => t.ClientSector).Returns("Private");
            var referenceDate = new DateTime(2021, 01, 01);

            var categories = new List<Category>
            {
                new Category { Name = "HIGHRISK", CompiledRule = (trade, date) => trade.Value > 1000000 && trade.ClientSector == "Private" }
            };

            _categoryServiceMock.Setup(cs => cs.GetCategoriesAsync(referenceDate)).ReturnsAsync(categories);

            // Act
            var result = await _categorizerService.Categorize(tradeMock.Object, referenceDate);

            // Assert
            Assert.Equal("HIGHRISK", result);
        }

        [Fact]
        public async Task Categorize_ShouldReturnMediumRisk()
        {
            // Arrange
            var tradeMock = new Mock<ITrade>();
            tradeMock.Setup(t => t.Value).Returns(2000000);
            tradeMock.Setup(t => t.ClientSector).Returns("Public");
            var referenceDate = new DateTime(2021, 01, 01);

            var categories = new List<Category>
            {
                new Category { Name = "MEDIUMRISK", CompiledRule = (trade, date) => trade.Value > 1000000 && trade.ClientSector == "Public" }
            };

            _categoryServiceMock.Setup(cs => cs.GetCategoriesAsync(referenceDate)).ReturnsAsync(categories);

            // Act
            var result = await _categorizerService.Categorize(tradeMock.Object, referenceDate);

            // Assert
            Assert.Equal("MEDIUMRISK", result);
        }

        [Fact]
        public async Task Categorize_ShouldReturnUndefined()
        {
            // Arrange
            var tradeMock = new Mock<ITrade>();
            var referenceDate = new DateTime(2021, 01, 01);

            var categories = new List<Category>();

            _categoryServiceMock.Setup(cs => cs.GetCategoriesAsync(referenceDate)).ReturnsAsync(categories);

            // Act
            var result = await _categorizerService.Categorize(tradeMock.Object, referenceDate);

            // Assert
            Assert.Equal("UNDEFINED", result);
        }
    }
}