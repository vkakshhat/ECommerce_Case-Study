using System.Collections.Generic;
using Entity_Library;

namespace DAO_Library
{
    public interface IOrderProcessorRepository
    {
        // Create and delete products
        bool CreateProduct(Products product);
        Products GetProductByName(string name);
        bool DeleteProduct(int productId);

        // Create and delete customers
        bool CreateCustomer(Customers customer);
        bool DeleteCustomer(int customerId);

        // Retrieve customer and product by their IDs
        Customers GetCustomerById(int customerId);
        Products GetProductById(int productId);

        // Cart operations
        bool AddToCart(Customers customer, Products product, int quantity);
        bool RemoveFromCart(Customers customer, Products product);
        List<(Products product, int quantity)> GetAllFromCart(Customers customer);

        // Order operations
        bool PlaceOrder(Customers customer, List<(Products product, int quantity)> cart, string shippingAddress);
        List<(Products product, int quantity)> GetOrdersByCustomer(int customerId);
    }
}
