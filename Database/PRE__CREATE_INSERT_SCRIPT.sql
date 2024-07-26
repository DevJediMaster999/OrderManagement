CREATE DATABASE OrderManagementDB_Test;
GO

USE OrderManagementDB_Test;

CREATE TABLE Shops (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(50) NULL,
    isEnabled BIT NOT NULL DEFAULT 1,
    dateCreated DATETIME NOT NULL DEFAULT GETDATE(),
    dateUpdated DATETIME NULL,
    idUserCreated INT NOT NULL,
    idUserUpdated INT NULL
);
GO
CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255) NULL,
    Price DECIMAL(18, 2) NOT NULL,
    isEnabled BIT NOT NULL DEFAULT 1,
    dateCreated DATETIME NOT NULL DEFAULT GETDATE(),
    dateUpdated DATETIME NULL,
    idUserCreated INT NOT NULL,
    idUserUpdated INT NULL
);
GO
CREATE TABLE ShopProducts (
    ShopId INT NOT NULL FOREIGN KEY REFERENCES Shops(Id),
    ProductId INT NOT NULL FOREIGN KEY REFERENCES Products(Id),
    PRIMARY KEY (ShopId, ProductId)
);
GO
CREATE TABLE OrderStatuses (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL
);
GO
CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ShopId INT NOT NULL FOREIGN KEY REFERENCES Shops(Id),
    OrderDate DATETIME NOT NULL,
    StatusId INT NOT NULL FOREIGN KEY REFERENCES OrderStatus(Id),
    TotalAmount DECIMAL(18, 2) NOT NULL,
    isEnabled BIT NOT NULL DEFAULT 1,
    dateCreated DATETIME NOT NULL DEFAULT GETDATE(),
    dateUpdated DATETIME NULL,
    idUserCreated INT NOT NULL,
    idUserUpdated INT NULL
);
GO
CREATE TABLE OrderProducts (
    OrderId INT NOT NULL FOREIGN KEY REFERENCES Orders(Id),
    ProductId INT NOT NULL FOREIGN KEY REFERENCES Products(Id),
    Quantity INT NOT NULL,
    PRIMARY KEY (OrderId, ProductId)
);

GO

INSERT INTO Shops (Name, Email, Phone, isEnabled, dateCreated, idUserCreated)
VALUES 
('Gadget Hub', 'contact@gadgethub.com', '123-456-7890', 1, GETDATE(), 1),
('Fashion Fiesta', 'info@fashionfiesta.com', '123-456-7891', 1, GETDATE(), 1),
('Book Haven', 'support@bookhaven.com', '123-456-7892', 1, GETDATE(), 1),
('Home Essentials', 'sales@homeessentials.com', '123-456-7893', 1, GETDATE(), 1);

INSERT INTO Products (Name, Description, Price, isEnabled, dateCreated, idUserCreated)
VALUES 
('Smartphone X', 'Latest model smartphone', 999.99, 1, GETDATE(), 1),
('Laptop Pro', 'High-performance laptop', 1999.99, 1, GETDATE(), 1),
('Wireless Earbuds', 'Noise-cancelling earbuds', 199.99, 1, GETDATE(), 1),
('Smartwatch 5', 'Advanced fitness smartwatch', 299.99, 1, GETDATE(), 1),
('Gaming Console', 'Next-gen gaming console', 499.99, 1, GETDATE(), 1),
('Designer Dress', 'Elegant designer dress', 149.99, 1, GETDATE(), 1),
('Leather Jacket', 'Stylish leather jacket', 249.99, 1, GETDATE(), 1),
('Cooking Pan Set', 'Non-stick cooking pan set', 89.99, 1, GETDATE(), 1),
('Bookshelf', 'Modern wooden bookshelf', 159.99, 1, GETDATE(), 1),
('LED Desk Lamp', 'Adjustable LED desk lamp', 39.99, 1, GETDATE(), 1);

INSERT INTO ShopProducts (ShopId, ProductId)
VALUES 
(1, 1),
(1, 2),
(1, 3),
(1, 4),
(1, 5),
(2, 6),
(2, 7),
(3, 8),
(3, 9),
(4, 10);

INSERT INTO OrderStatus (Name) VALUES 
('New'), 
('Processing'), 
('Completed'), 
('Canceled');

INSERT INTO Orders (ShopId, OrderDate, StatusId, TotalAmount, isEnabled, dateCreated, idUserCreated)
VALUES 
(1, GETDATE(), 1, 999.99, 1, GETDATE(), 1),
(2, GETDATE(), 1, 399.98, 1, GETDATE(), 1),
(3, GETDATE(), 1, 249.95, 1, GETDATE(), 1),
(4, GETDATE(), 1, 129.98, 1, GETDATE(), 1);

INSERT INTO OrderProducts (OrderId, ProductId, Quantity)
VALUES 
(1, 1, 1),
(2, 6, 2),
(3, 8, 1),
(3, 9, 1),
(4, 10, 1);
