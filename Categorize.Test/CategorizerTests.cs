using Categorize.Application.Interfaces;
using Categorize.Application.Services;
using Categorize.Domain.Categories;
using Categorize.Domain.Interfaces;
using Moq;

namespace Categorize.Tests
{
    public class CategorizerTests
    {
        private readonly Mock<ICategory> _expiredCategoryMock;
        private readonly Mock<ICategory> _highRiskCategoryMock;
        private readonly Mock<ICategory> _mediumRiskCategoryMock;
        private readonly ICategorizerService _categorizerService;

        public CategorizerTests()
        {
            _expiredCategoryMock = new Mock<ICategory>();
            _expiredCategoryMock.Setup(c => c.Name).Returns("EXPIRED");

            _highRiskCategoryMock = new Mock<ICategory>();
            _highRiskCategoryMock.Setup(c => c.Name).Returns("HIGHRISK");

            _mediumRiskCategoryMock = new Mock<ICategory>();
            _mediumRiskCategoryMock.Setup(c => c.Name).Returns("MEDIUMRISK");

            var categories = new List<ICategory>
            {
                _expiredCategoryMock.Object,
                _highRiskCategoryMock.Object,
                _mediumRiskCategoryMock.Object
            };

            _categorizerService = new CategorizerService(categories);
        }

        [Fact]
        public void Categorize_ShouldReturnExpired()
        {
            // Arrange
            var tradeMock = new Mock<ITrade>();
            tradeMock.Setup(t => t.NextPaymentDate).Returns(new DateTime(2020, 11, 01));

            var referenceDate = new DateTime(2021, 01, 01);

            _expiredCategoryMock.Setup(c => c.IsMatch(tradeMock.Object, referenceDate)).Returns(true);

            // Act
            var result = _categorizerService.Categorize(tradeMock.Object, referenceDate);

            // Assert
            Assert.Equal("EXPIRED", result);
        }

        [Fact]
        public void Categorize_ShouldReturnHighRisk()
        {
            // Arrange
            var tradeMock = new Mock<ITrade>();
            var referenceDate = new DateTime(2021, 01, 01);

            _expiredCategoryMock.Setup(c => c.IsMatch(tradeMock.Object, referenceDate)).Returns(false);
            _highRiskCategoryMock.Setup(c => c.IsMatch(tradeMock.Object, referenceDate)).Returns(true);
            _mediumRiskCategoryMock.Setup(c => c.IsMatch(tradeMock.Object, referenceDate)).Returns(false);

            // Act
            var result = _categorizerService.Categorize(tradeMock.Object, referenceDate);

            // Assert
            Assert.Equal("HIGHRISK", result);
        }

        [Fact]
        public void Categorize_ShouldReturnMediumRisk()
        {
            // Arrange
            var tradeMock = new Mock<ITrade>();
            var referenceDate = new DateTime(2021, 01, 01);

            _expiredCategoryMock.Setup(c => c.IsMatch(tradeMock.Object, referenceDate)).Returns(false);
            _highRiskCategoryMock.Setup(c => c.IsMatch(tradeMock.Object, referenceDate)).Returns(false);
            _mediumRiskCategoryMock.Setup(c => c.IsMatch(tradeMock.Object, referenceDate)).Returns(true);

            // Act
            var result = _categorizerService.Categorize(tradeMock.Object, referenceDate);

            // Assert
            Assert.Equal("MEDIUMRISK", result);
        }

        [Fact]
        public void Categorize_ShouldReturnUndefined()
        {
            // Arrange
            var tradeMock = new Mock<ITrade>();
            var referenceDate = new DateTime(2021, 01, 01);

            _expiredCategoryMock.Setup(c => c.IsMatch(tradeMock.Object, referenceDate)).Returns(false);
            _highRiskCategoryMock.Setup(c => c.IsMatch(tradeMock.Object, referenceDate)).Returns(false);
            _mediumRiskCategoryMock.Setup(c => c.IsMatch(tradeMock.Object, referenceDate)).Returns(false);

            // Act
            var result = _categorizerService.Categorize(tradeMock.Object, referenceDate);

            // Assert
            Assert.Equal("UNDEFINED", result);
        }
    }
}