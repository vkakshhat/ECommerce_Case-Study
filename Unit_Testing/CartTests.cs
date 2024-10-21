using Entity_Library;
using NUnit.Framework;

namespace Unit_Testing
{
    public class CartTests : TestBase
    {
        [Test]
        public void TestProductAddedToCart()
        {
            var product = new Products
            {
                Name = "Test Product",
                Price = 99.99M,
                Description = "This is a test product.",
                StockQuantity = 10 
            };

            var productCreated = _orderProcessorRepository.CreateProduct(product);

            Assert.That(productCreated, Is.True, "The product should be created successfully.");

            var createdProduct = _orderProcessorRepository.GetProductByName(product.Name); 

            var customer = new Customers
            {
                Customer_id = 1 
            };

            var result = _orderProcessorRepository.AddToCart(customer, createdProduct, 1);

            Assert.That(result, Is.True, "Product should be added to the cart successfully.");
        }
    }
}
