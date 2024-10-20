using NUnit.Framework;
using DAO_Library;
using Entity_Library;
using System;
using Exception_Library;

namespace Unit_Testing
{
    public class ExceptionTests : TestBase
    {
        [Test]
        public void TestCustomerNotFoundException()
        {
            // Arrange
            var customerId = -1; // Invalid customer ID

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _orderProcessorRepository.GetCustomerById(customerId));
            Assert.That(ex.Message, Is.EqualTo("Customer not found.")); // Check for the specific message
        }

        [Test]
        public void TestProductNotFoundException()
        {
            // Arrange
            var productId = -1; // Invalid product ID

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _orderProcessorRepository.GetProductById(productId));
            Assert.That(ex.Message, Is.EqualTo("Product not found.")); // Check for the specific message
        }
    }
}
