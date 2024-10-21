-- Case Study on Ecommerce Application
CREATE DATABASE EcomApp;;
USE EcomApp;

-- Creating customers table
CREATE TABLE Customers (
    Customer_id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,  
    Password VARCHAR(255) NOT NULL
);

-- Creating products table
CREATE TABLE Products (
    Product_id INT IDENTITY(1,1) PRIMARY KEY,
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
    Quantity INT NOT NULL CHECK (Quantity > 0),  
    FOREIGN KEY (Customer_id) REFERENCES Customers(Customer_id),
    FOREIGN KEY (Product_id) REFERENCES Products(Product_id) ON DELETE CASCADE  -- Cascade delete on product
);


-- Creating orders table
CREATE TABLE Orders (
    Order_id INT IDENTITY(1,1) PRIMARY KEY,  
    Customer_id INT NOT NULL,
    Order_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    Total_price DECIMAL(10, 2) CHECK (Total_price >= 0),  
    Shipping_address VARCHAR(255),
    FOREIGN KEY (Customer_id) REFERENCES Customers(Customer_id)
);

-- Creating order_items table
CREATE TABLE Order_items (
    Order_item_id INT IDENTITY(1,1) PRIMARY KEY,  
    Order_id INT NOT NULL,
    Product_id INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),  
    FOREIGN KEY (Order_id) REFERENCES Orders(Order_id),
    FOREIGN KEY (Product_id) REFERENCES Products(Product_id) ON DELETE CASCADE
);

-- Inserting 5 customers into Customers table 

INSERT INTO Customers (Name, Email, Password) 
VALUES 
('Ravi Kumar', 'ravi.kumar@example.in', 'ravi123'),
('Priya Sharma', 'priya.sharma@example.in', 'priya456'),
('Amit Patel', 'amit.patel@example.in', 'amit789'),
('Neha Singh', 'neha.singh@example.in', 'neha000'),
('Rajesh Gupta', 'rajesh.gupta@example.in', 'gupta999');


-- Inserting 5 products into Products table 
INSERT INTO Products (Name, Price, Description, StockQuantity) 
VALUES 
('Smartphone', 15000.00, 'Android smartphone with 6GB RAM', 50),
('LED TV', 30000.00, '42-inch LED TV with 4K resolution', 20),
('Refrigerator', 25000.00, 'Double door refrigerator', 10),
('Washing Machine', 18000.00, 'Front load washing machine', 15),
('Microwave Oven', 8000.00, 'Solo microwave oven with 20L capacity', 25);

-- Inserting records into Cart table with correct Customer_id and Product_id values
INSERT INTO Cart (Customer_id, Product_id, Quantity)
VALUES
(1, 1, 4),  
(2, 2, 5),  
(3, 3, 3),  
(4, 4, 1),  
(5, 5, 2);  

-- Inserting records into Orders table
INSERT INTO Orders (Customer_id, Total_price, Shipping_address)
VALUES
(1, 2 * 15000.00, 'Mumbai, Maharashtra'),        
(2, 1 * 25000.00, 'Delhi, Delhi'),              
(3, 3 * 8000.00, 'Ahmedabad, Gujarat'),          
(4, 1 * 30000.00, 'Lucknow, Uttar Pradesh'),      
(5, 1 * 18000.00, 'Kolkata, West Bengal');       

-- Inserting records into Order_items table with updated Order_id values
INSERT INTO Order_items (Order_id, Product_id, Quantity)
VALUES
(1, 1, 2), 
(2, 2, 1), 
(3, 3, 3),  
(4, 4, 1), 
(5, 5, 1); 

select* from Customers
select* from Products
select* from Cart
select * from Orders
select * from Order_items
