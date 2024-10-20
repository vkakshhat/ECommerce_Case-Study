using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Entity_Library;
using Utility_Library;
using Exception_Library;

namespace DAO_Library
{
    public class OrderProcessorRepositoryImpl : IOrderProcessorRepository
    {
        private readonly SqlConnection _connection;

        public OrderProcessorRepositoryImpl(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        // Create a new product
        public bool CreateProduct(Products product)
        {
            string query = "INSERT INTO Products (Name, Price, Description, StockQuantity) VALUES (@name, @price, @description, @stockQuantity)";

            try
            {
                // Ensure the connection is open
                if (_connection.State == System.Data.ConnectionState.Closed)
                {
                    _connection.Open();
                }

                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@name", product.Name);
                    cmd.Parameters.AddWithValue("@price", product.Price);
                    cmd.Parameters.AddWithValue("@description", product.Description);
                    cmd.Parameters.AddWithValue("@stockQuantity", product.StockQuantity);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            finally
            {
                // Ensure the connection is closed after use
                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
        }

        // Create a new customer
        public bool CreateCustomer(Customers customer)
        {
            string query = "INSERT INTO Customers (Name, Email, Password) VALUES (@name, @Email, @Password)";

            try
            {
                // Ensure the connection is open
                if (_connection.State == System.Data.ConnectionState.Closed)
                {
                    _connection.Open();
                }

                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@name", customer.Name);
                    cmd.Parameters.AddWithValue("@Email", customer.Email);
                    cmd.Parameters.AddWithValue("@Password", customer.Password);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            finally
            {
                // Ensure the connection is closed after use
                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
        }

        // Delete a product by product ID
        public bool DeleteProduct(int productId)
        {
            string query = "DELETE FROM Products WHERE Product_id = @productId";

            try
            {
                // Ensure the connection is open
                if (_connection.State == System.Data.ConnectionState.Closed)
                {
                    _connection.Open();
                }

                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@productId", productId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            finally
            {
                // Ensure the connection is closed after use
                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
        }

        // Delete a customer by customer ID
        public bool DeleteCustomer(int customerId)
        {
            string query = "DELETE FROM Customers WHERE Customer_id = @customerId";

            try
            {
                // Ensure the connection is open
                if (_connection.State == System.Data.ConnectionState.Closed)
                {
                    _connection.Open();
                }

                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            finally
            {
                // Ensure the connection is closed after use
                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
        }


        // Add a product to the customer's cart
        public bool AddToCart(Customers customer, Products product, int quantity)
        {
            try
            {
                // Ensure the connection is open
                if (_connection.State == System.Data.ConnectionState.Closed)
                {
                    _connection.Open();
                }

                // Check if enough stock is available
                if (product.StockQuantity < quantity)
                {
                    Console.WriteLine("Error: Not enough stock available!");
                    return false; // Not enough stock
                }

                // Check if the product already exists in the cart
                string checkQuery = "SELECT Quantity FROM Cart WHERE Customer_id = @customerId AND Product_id = @productId";
                int currentQuantity = 0;

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, _connection))
                {
                    checkCmd.Parameters.AddWithValue("@customerId", customer.Customer_id);
                    checkCmd.Parameters.AddWithValue("@productId", product.Product_id);

                    var result = checkCmd.ExecuteScalar();
                    if (result != null)
                    {
                        currentQuantity = Convert.ToInt32(result);
                    }
                }

                // If the product exists in the cart, update the quantity
                if (currentQuantity > 0)
                {
                    string updateCartQuery = "UPDATE Cart SET Quantity = Quantity + @quantity WHERE Customer_id = @customerId AND Product_id = @productId";
                    using (SqlCommand updateCartCmd = new SqlCommand(updateCartQuery, _connection))
                    {
                        updateCartCmd.Parameters.AddWithValue("@quantity", quantity);
                        updateCartCmd.Parameters.AddWithValue("@customerId", customer.Customer_id);
                        updateCartCmd.Parameters.AddWithValue("@productId", product.Product_id);
                        updateCartCmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    // Product does not exist in the cart, insert a new entry
                    string insertQuery = "INSERT INTO Cart (Customer_id, Product_id, Quantity) VALUES (@customerId, @productId, @quantity)";
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, _connection))
                    {
                        insertCmd.Parameters.AddWithValue("@customerId", customer.Customer_id);
                        insertCmd.Parameters.AddWithValue("@productId", product.Product_id);
                        insertCmd.Parameters.AddWithValue("@quantity", quantity);
                        insertCmd.ExecuteNonQuery();
                    }
                }

                // Update stock quantity in Products table
                string updateStockQuery = "UPDATE Products SET StockQuantity = StockQuantity - @quantity WHERE Product_id = @productId";
                using (SqlCommand updateStockCmd = new SqlCommand(updateStockQuery, _connection))
                {
                    updateStockCmd.Parameters.AddWithValue("@quantity", quantity);
                    updateStockCmd.Parameters.AddWithValue("@productId", product.Product_id);
                    updateStockCmd.ExecuteNonQuery();
                }

                return true; // Successfully added to cart or updated quantity
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false; // Error occurred while adding to cart
            }
            finally
            {
                // Ensure the connection is closed
                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
        }

        // Remove a product from the customer's cart
        public bool RemoveFromCart(Customers customer, Products product)
        {
            string query = "DELETE FROM Cart WHERE Customer_id = @customerId AND Product_id = @productId";
            using (SqlCommand cmd = new SqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@customerId", customer.Customer_id);
                cmd.Parameters.AddWithValue("@productId", product.Product_id);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Get all products from the cart for a specific customer
        public List<(Products product, int quantity)> GetAllFromCart(Customers customer)
        {
            List<(Products product, int quantity)> cartItems = new List<(Products product, int quantity)>();

            // Open connection if it is not already open
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
            }

            string query = "SELECT p.Product_id, p.Name, p.Price, c.Quantity " +
                           "FROM Cart c JOIN Products p ON c.Product_id = p.Product_id " +
                           "WHERE c.Customer_id = @customerId";

            using (SqlCommand cmd = new SqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@customerId", customer.Customer_id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Products product = new Products
                        {
                            Product_id = Convert.ToInt32(reader["Product_id"]),
                            Name = reader["Name"].ToString(),
                            Price = Convert.ToDecimal(reader["Price"])
                        };
                        int quantity = Convert.ToInt32(reader["Quantity"]);
                        cartItems.Add((product, quantity)); // Add as tuple
                    }
                }
            }
            return cartItems;
        }
        public bool UpdateProductStock(int productId, int newStock)
        {
            try
            {
                string query = "UPDATE Products SET Stock = @newStock WHERE Product_id = @productId";

                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@newStock", newStock);
                    cmd.Parameters.AddWithValue("@productId", productId);

                    // Open the connection if it is not already open
                    if (_connection.State == System.Data.ConnectionState.Closed)
                    {
                        _connection.Open();
                    }

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0; // Return true if the update was successful
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"Error updating product stock: {ex.Message}");
                return false;
            }
            finally
            {
                // Ensure the connection is closed after the operation
                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
        }


        // Place an order
        public bool PlaceOrder(Customers customer, List<(Products product, int quantity)> cart, string shippingAddress)
        {
            // Make sure the connection is available before proceeding
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
            }

            SqlTransaction transaction = null;
            try
            {
                transaction = _connection.BeginTransaction();

                // Debugging information
                Console.WriteLine("----------------------------------------");
                Console.WriteLine($"Customer ID: {customer.Customer_id}");
                Console.WriteLine($"Total Price: {CalculateTotalPrice(cart)}");
                Console.WriteLine($"Shipping Address: {shippingAddress}");
                Console.WriteLine("----------------------------------------");

                // Insert into Orders table
                string orderQuery = "INSERT INTO Orders (Customer_id, Total_price, Shipping_address) " +
                                    "VALUES (@customerId, @totalPrice, @shippingAddress); SELECT SCOPE_IDENTITY();";
                using (SqlCommand orderCmd = new SqlCommand(orderQuery, _connection, transaction))
                {
                    orderCmd.Parameters.AddWithValue("@customerId", customer.Customer_id);
                    orderCmd.Parameters.AddWithValue("@totalPrice", CalculateTotalPrice(cart));
                    orderCmd.Parameters.AddWithValue("@shippingAddress", shippingAddress);
                    int orderId = Convert.ToInt32(orderCmd.ExecuteScalar());

                    // Insert into Order_Items table
                    foreach (var item in cart)
                    {
                        string itemQuery = "INSERT INTO Order_items (Order_id, Product_id, Quantity) " +
                                           "VALUES (@orderId, @productId, @quantity)";
                        using (SqlCommand itemCmd = new SqlCommand(itemQuery, _connection, transaction))
                        {
                            itemCmd.Parameters.AddWithValue("@orderId", orderId);
                            itemCmd.Parameters.AddWithValue("@productId", item.product.Product_id);
                            itemCmd.Parameters.AddWithValue("@quantity", item.quantity);
                            itemCmd.ExecuteNonQuery();
                        }
                    }

                    // Clear the customer's cart after placing the order
                    string clearCartQuery = "DELETE FROM Cart WHERE Customer_id = @customerId";
                    using (SqlCommand clearCartCmd = new SqlCommand(clearCartQuery, _connection, transaction))
                    {
                        clearCartCmd.Parameters.AddWithValue("@customerId", customer.Customer_id);
                        clearCartCmd.ExecuteNonQuery();
                    }

                    // Commit the transaction
                    transaction.Commit();
                    return true; // Successfully placed the order
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                transaction?.Rollback(); // Rollback the transaction in case of error
                return false;
            }
            finally
            {
                // Close the connection only if you are done with all operations
                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
        }

        // Get orders by customer ID
        public List<(Products product, int quantity)> GetOrdersByCustomer(int customerId)
        {
            // Initialize a list to hold the order details as tuples
            List<(Products product, int quantity)> orders = new List<(Products product, int quantity)>();

            // Query to get product orders for a specific customer
            string query = "SELECT p.Product_id, p.Name, p.Price, oi.Quantity FROM Order_items oi " +
                           "JOIN Products p ON oi.Product_id = p.Product_id " +
                           "WHERE oi.Order_id IN (SELECT o.Order_id FROM Orders o WHERE o.Customer_id = @customerId)";

            // Ensure the connection is open before executing the command
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open(); // Open the connection if it's not already open
            }

            using (SqlCommand cmd = new SqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@customerId", customerId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int productId = reader.GetInt32(0);
                        string productName = reader.GetString(1);
                        decimal productPrice = reader.GetDecimal(2); // Assuming price is a decimal
                        int quantity = reader.GetInt32(3);

                        // Create a Products object from the data retrieved
                        Products product = new Products
                        {
                            Product_id = productId,
                            Name = productName,
                            Price = productPrice, // Add Price property to the Products object
                                                  // Populate other properties as needed
                        };

                        // Add the tuple (product, quantity) to the orders list
                        orders.Add((product, quantity));
                    }
                }
            }

            // Optionally close the connection after the command execution if it was opened by this method
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }

            return orders;
        }


        // Helper method to calculate total price of the cart
        public decimal CalculateTotalPrice(List<(Products product, int quantity)> cart)
        {
            decimal totalPrice = 0.0m;

            foreach (var item in cart)
            {
                totalPrice += item.product.Price * item.quantity; // Assuming product has a Price property
            }

            return totalPrice;
        }


        // Get customer by ID (throw exception if not found)
        public Customers GetCustomerById(int customerId)
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
            }

            string query = "SELECT * FROM Customers WHERE Customer_id = @customerId";
            using (SqlCommand cmd = new SqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@customerId", customerId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Map the customer from the database
                        return new Customers
                        {
                            Customer_id = reader.GetInt32(0),
                            // Populate other properties as needed
                        };
                    }
                    else
                    {
                        throw new Exception("Customer not found."); // Throw an exception if the customer is not found
                    }
                }
            }
        }


        // Get product by ID (throw exception if not found)
        public Products GetProductById(int productId)
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
            }

            string query = "SELECT * FROM Products WHERE Product_id = @productId";
            using (SqlCommand cmd = new SqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@productId", productId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Map the product from the database
                        return new Products
                        {
                            Product_id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            // Cast StockQuantity from decimal to int if needed
                            StockQuantity = Convert.ToInt32(reader.GetDecimal(2)),  // Adjust index based on actual column order
                                                                                    // Populate other properties as needed
                        };
                    }
                    else
                    {
                        throw new Exception("Product not found."); // Throw an exception if the product is not found
                    }
                }
            }
        }

        //Get product by name
        public Products GetProductByName(string name)
        {
            string query = "SELECT * FROM Products WHERE Name = @name";

            using (SqlCommand cmd = new SqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@name", name);

                // Ensure the connection is open
                if (_connection.State == System.Data.ConnectionState.Closed)
                {
                    _connection.Open();
                }

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Products
                        {
                            Product_id = (int)reader["Product_id"],
                            Name = (string)reader["Name"],
                            Price = (decimal)reader["Price"],
                            Description = (string)reader["Description"],
                            StockQuantity = (int)reader["StockQuantity"]
                        };
                    }
                }
            }

            return null; // Return null if not found
        }


    }
}
