using Entity_Library;
using NUnit.Framework;

namespace Unit_Testing
{
    public class ProductTests : TestBase
    {
        [Test]
        public void TestProductCreation()
        {
            var product = new Products
            {
                Name = "Test Product",
                Price = 99.99M,
                Description = "This is a test product.", 
                StockQuantity = 10 
            };

            var result = _orderProcessorRepository.CreateProduct(product);

            Assert.That(result, Is.True, "Product should be created successfully.");
        }
    }
}
