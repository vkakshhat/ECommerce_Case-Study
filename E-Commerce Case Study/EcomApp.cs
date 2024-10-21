using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DAO_Library;
using Entity_Library;
using Exception_Library;
using Microsoft.Extensions.Configuration;

namespace ECommerce
{
    class EcomApp
    {
        static void Main(string[] args)
        {
            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Retrieve the connection string
            string connectionString = configuration.GetConnectionString("dbCn");

            // Pass the connection string to classes
            var orderProcessorRepository = new OrderProcessorRepositoryImpl(connectionString);

        bool exit = false;
            Console.WriteLine("Welcome to E-Commerce App!");
            while (!exit)
            {
                // Display the menu
                Console.WriteLine("");
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine("");
                Console.WriteLine("Please choose an operation:");
                Console.WriteLine("");
                Console.WriteLine("1. Register Customer");
                Console.WriteLine("2. Create Product");
                Console.WriteLine("3. Delete Product");
                Console.WriteLine("4. Add to Cart");
                Console.WriteLine("5. View Cart");
                Console.WriteLine("6. Place Order");
                Console.WriteLine("7. View Customer Order");
                Console.WriteLine("8. Exit");
                Console.WriteLine("");
                Console.WriteLine("-----------------------------------------");


                int choice = 0;
                bool validChoice = false;

                // Validate user input
                while (!validChoice)
                {
                    try
                    {
                        Console.WriteLine("");
                        Console.Write("Enter your choice: ");

                        choice = Convert.ToInt32(Console.ReadLine());

                        if (choice < 1 || choice > 8)
                        {
                            Console.WriteLine("");
                            throw new Exception("Invalid choice. Please enter a valid number between 1 and 8.");
                        }
                        validChoice = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                Console.WriteLine("");
                Console.WriteLine("-----------------------------------------");


                switch (choice)
                {
                    case 1:
                        RegisterCustomer(orderProcessorRepository);
                        break;

                    case 2:
                        CreateProduct(orderProcessorRepository);
                        break;

                    case 3:
                        DeleteProduct(orderProcessorRepository);
                        break;

                    case 4:
                        AddToCart(orderProcessorRepository);
                        break;

                    case 5:
                        ViewCart(orderProcessorRepository);
                        break;

                    case 6:
                        PlaceOrder(orderProcessorRepository);
                        break;

                    case 7:
                        ViewCustomerOrder(orderProcessorRepository);
                        break;

                    case 8:
                        exit = true;
                        Console.WriteLine("");
                        Console.WriteLine("Exiting the application. Goodbye!");
                        break;
                }
            }
        }

        // Method to Register Customer
        static void RegisterCustomer(IOrderProcessorRepository orderProcessorRepository)
        {
            try
            {
                Console.WriteLine("");
                Console.WriteLine("Enter Customer Details:");
                Console.WriteLine("");

                Console.Write("Name: ");
                string name = Console.ReadLine();

                Console.Write("Email: ");
                string email = Console.ReadLine();

                Console.Write("Password: ");
                string password = Console.ReadLine();

                Customers customer = new Customers
                {
                    Name = name,
                    Email = email,
                    Password = password
                };

                // Call the RegisterCustomer method in OrderProcessorImpl
                bool success = orderProcessorRepository.CreateCustomer(customer);

                if (success)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Customer Registered Successfully!");
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("Error: Customer Registration Failed!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Method to Create Product
        static void CreateProduct(IOrderProcessorRepository orderProcessorRepository)
        {
            try
            {
                Console.WriteLine("");
                Console.WriteLine("Enter Product Details:");
                Console.WriteLine("")
                    ;
                Console.Write("Product Name: ");
                string name = Console.ReadLine();

                Console.Write("Price: ");
                decimal price = Convert.ToDecimal(Console.ReadLine());

                Console.Write("Description: ");
                string description = Console.ReadLine();

                Console.Write("Stock Quantity: ");
                int stockQuantity = Convert.ToInt32(Console.ReadLine());

                Products product = new Products
                {
                    Name = name,
                    Price = price,
                    Description = description,
                    StockQuantity = stockQuantity
                };

                bool success = orderProcessorRepository.CreateProduct(product);
                if (success)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Product Created Successfully!");
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("Error: Product Creation Failed!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Method to Delete Product
        static void DeleteProduct(IOrderProcessorRepository orderProcessorRepository)
        {
            try
            {
                Console.WriteLine("");
                Console.Write("Enter Product ID to delete: ");
                int productId = Convert.ToInt32(Console.ReadLine());

                // Proceed with deletion
                bool success = orderProcessorRepository.DeleteProduct(productId);
                if (success)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Product Deleted Successfully!");
                }
                else
                {
                    Console.WriteLine("");
                    throw new ProductNotFoundException("Error: Failed to Delete the Product.");
                }
            }
            catch (ProductNotFoundException ex)
            {
                Console.WriteLine("");
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


        // Method to Add to Cart
        static void AddToCart(IOrderProcessorRepository orderProcessorRepository)
        {
            try
            {
                Console.WriteLine("");
                // Get Customer ID
                Console.Write("Enter Customer ID: ");
                int customerId = Convert.ToInt32(Console.ReadLine());

                // Fetch the customer by ID
                Customers customer = orderProcessorRepository.GetCustomerById(customerId);
                if (customer == null)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Error: Customer Not Found!");
                    return;
                }

                // Get Product ID
                Console.Write("Enter Product ID: ");
                int productId = Convert.ToInt32(Console.ReadLine());

                // Fetch the product by ID
                Products product = orderProcessorRepository.GetProductById(productId);
                if (product == null)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Error: Product Not Found!");
                    return;
                }

                // Get the quantity to be added to the cart
                Console.Write("Enter Quantity: ");
                int quantity = Convert.ToInt32(Console.ReadLine());

                // Check if there is enough stock for the product
                if (product.StockQuantity < quantity)
                {
                    Console.WriteLine("");
                    Console.WriteLine($"Error: Not enough stock available! Available stock: {product.StockQuantity}");
                    return;
                }

                // Try to add the product to the cart
                bool success = orderProcessorRepository.AddToCart(customer, product, quantity);
                if (success)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Product Added to Cart Successfully!");
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("Error: Failed to Add Product to Cart!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


        // Method to View Cart
        static void ViewCart(IOrderProcessorRepository orderProcessorRepository)
        {
            try
            {
                Console.WriteLine("");
                Console.Write("Enter Customer ID to view cart: ");
                int customerId = Convert.ToInt32(Console.ReadLine());

                Customers customer = orderProcessorRepository.GetCustomerById(customerId);
                if (customer == null)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Error: Customer Not Found!");
                    return;
                }

                // Get cart items along with their quantities
                List<(Products product, int quantity)> cartItems = orderProcessorRepository.GetAllFromCart(customer);
                if (cartItems.Count == 0)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Cart is Empty!");
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("Items in Cart:");
                    foreach (var item in cartItems)
                    {
                        Console.WriteLine($"Product: {item.product.Name}, Price: {item.product.Price}, Quantity: {item.quantity}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Method to Place Order
        static void PlaceOrder(IOrderProcessorRepository orderProcessorRepository)
        {
            try
            {
                Console.WriteLine("");
                Console.Write("Enter Customer ID to place order: ");
                int customerId = Convert.ToInt32(Console.ReadLine());

                // Fetch the customer by ID
                Customers customer = orderProcessorRepository.GetCustomerById(customerId);
                if (customer == null)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Error: Customer Not Found!");
                    return;
                }

                Console.Write("Enter Shipping Address: ");
                string shippingAddress = Console.ReadLine();

                // Fetch the cart items in the correct format
                List<(Products product, int quantity)> cart = orderProcessorRepository.GetAllFromCart(customer);

                // Check if the cart is empty before placing the order
                if (cart.Count == 0)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Error: Your cart is empty! Please add items before placing an order.");
                    return;
                }

                // Pass the correct type to PlaceOrder
                bool success = orderProcessorRepository.PlaceOrder(customer, cart, shippingAddress);
                if (success)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Order Placed Successfully!");
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("Error: Failed to Place Order!");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("");
                Console.WriteLine("Error: Please enter a valid Customer ID.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Method to View Customer Orders
        static void ViewCustomerOrder(IOrderProcessorRepository orderProcessorRepository)
        {
            try
            {
                Console.WriteLine("");
                Console.Write("Enter Customer ID to view orders: ");
                int customerId = Convert.ToInt32(Console.ReadLine());

                List<(Products product, int quantity)> orders = orderProcessorRepository.GetOrdersByCustomer(customerId);
                if (orders.Count == 0)
                {
                    Console.WriteLine("");
                    Console.WriteLine("No Orders Found for Customer!");
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("Customer Orders:");
                    foreach (var order in orders)
                    {
                        Console.WriteLine($"Product: {order.product.Name}, Quantity: {order.quantity}, Price: {order.product.Price}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
