using Entity_Library;
using NUnit.Framework;

namespace Unit_Testing
{
    public class CartTests : TestBase
    {
        [Test]
        public void TestProductAddedToCart()
        {
            // Arrange
            var product = new Products
            {
                Name = "Test Product",
                Price = 99.99M,
                Description = "This is a test product.",
                StockQuantity = 10 // This should be greater than 0
            };

            // Create the product in the database first
            var productCreated = _orderProcessorRepository.CreateProduct(product);

            // Verify the product was created
            Assert.That(productCreated, Is.True, "The product should be created successfully.");

            // Retrieve the product ID from the database (assuming CreateProduct sets it)
            var createdProduct = _orderProcessorRepository.GetProductByName(product.Name); // You'll need to implement this method to get the product

            var customer = new Customers
            {
                Customer_id = 1 // Ensure this ID exists in your database
            };

            // Act
            var result = _orderProcessorRepository.AddToCart(customer, createdProduct, 1); // Attempt to add one item

            // Assert
            Assert.That(result, Is.True, "Product should be added to the cart successfully.");
        }
    }
}
