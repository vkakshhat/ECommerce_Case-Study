using Entity_Library;
using NUnit.Framework;

namespace Unit_Testing
{
    public class ProductTests : TestBase
    {
        [Test]
        public void TestProductCreation()
        {
            // Arrange
            var product = new Products
            {
                Name = "Test Product",
                Price = 99.99M,
                Description = "This is a test product.", // Ensure description is set
                StockQuantity = 10 // Ensure stock quantity is set
            };

            // Act
            var result = _orderProcessorRepository.CreateProduct(product);

            // Assert
            Assert.That(result, Is.True, "Product should be created successfully.");
        }
    }
}
