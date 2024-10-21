-- Case Study on Ecommerce Application
CREATE DATABASE EcomApp;;
USE EcomApp;

-- Creating customers table
CREATE TABLE Customers (
    Customer_id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,  -- Ensuring email is unique
    Password VARCHAR(255) NOT NULL
);

-- Creating products table
CREATE TABLE Products (
    Product_id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(50),
    Price DECIMAL(18, 2),
    Description NVARCHAR(255),
    StockQuantity INT
);

-- Creating cart table
CREATE TABLE Cart (
    Cart_id INT IDENTITY(1,1) PRIMARY KEY,
    Customer_id INT NOT NULL,
    Product_id INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),  -- Ensuring quantity is positive
    FOREIGN KEY (Customer_id) REFERENCES Customers(Customer_id),
    FOREIGN KEY (Product_id) REFERENCES Products(Product_id) ON DELETE CASCADE  -- Cascade delete on product
);


-- Creating orders table
CREATE TABLE Orders (
    Order_id INT IDENTITY(1,1) PRIMARY KEY,  -- Set Order_id to auto-increment
    Customer_id INT NOT NULL,
    Order_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    Total_price DECIMAL(10, 2) CHECK (Total_price >= 0),  -- Ensuring total price is non-negative
    Shipping_address VARCHAR(255),
    FOREIGN KEY (Customer_id) REFERENCES Customers(Customer_id)
);

-- Creating order_items table
CREATE TABLE Order_items (
    Order_item_id INT IDENTITY(1,1) PRIMARY KEY,  -- Set Order_item_id to auto-increment
    Order_id INT NOT NULL,
    Product_id INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),  -- Ensuring quantity is positive
    FOREIGN KEY (Order_id) REFERENCES Orders(Order_id),
    FOREIGN KEY (Product_id) REFERENCES Products(Product_id)
);

select* from Customers
select* from Products
select * from Orders
select * from Order_items
select* from Cart

