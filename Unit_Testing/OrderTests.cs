using NUnit.Framework;
using Entity_Library; // Reference to entity classes
using DAO_Library; // Reference to repository classes

namespace Unit_Testing
{
    public class OrderTests : TestBase
    {
        [Test]
        public void TestProductOrderedSuccessfully()
        {
            // Arrange
            var customer = new Customers { Customer_id = 1 }; // Ensure this customer exists
            var product = new Products { Product_id = 1, Name = "Test Product", Price = 100.00m };
            var cart = new List<(Products product, int quantity)>
            {
                (product, 1)
            };
            string shippingAddress = "123 Test St, Test City";

            // Act
            bool result = _orderProcessorRepository.PlaceOrder(customer, cart, shippingAddress);

            // Assert
            Assert.That(result, Is.True, "Product should be ordered successfully.");
        }
    }
}
