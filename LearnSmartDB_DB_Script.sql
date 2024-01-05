
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'LearnSmartDB')
BEGIN
    -- Create the database
    CREATE DATABASE LearnSmartDB;
END
ELSE
BEGIN
   DROP DATABASE LearnSmartDB;
END

Go
use LearnSmartDB
go

-- User Profile Table
CREATE TABLE UserProfile (
    UserId INT IDENTITY(1,1),
	DisplayName NVARCHAR(100) NOT NULL CONSTRAINT DF_UserProfile_DisplayName DEFAULT 'Guest',
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
	AdObjId NVARCHAR(128) NOT NULL,
    -- Add other user-related fields as needed
    CONSTRAINT PK_UserProfile_UserId PRIMARY KEY (UserId)
);

--Roles
CREATE TABLE Roles (
    RoleId INT IDENTITY(1,1),    
    RoleName NVARCHAR(50) NOT NULL, --Admin, ReadOnly, Support, etc    
    CONSTRAINT PK_Roles_RoleId PRIMARY KEY (RoleId)    
);

-- UserRole Table
CREATE TABLE UserRole (
    UserRoleId INT IDENTITY(1,1),
	RoleId INT,
    UserId INT,        
    CONSTRAINT PK_UserRole_UserRoleId PRIMARY KEY (UserRoleId),
    CONSTRAINT FK_UserRole_UserProfile FOREIGN KEY (UserId) REFERENCES UserProfile(UserId),
	 CONSTRAINT FK_UserRole_Roles FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
);


-- Product Category Table
CREATE TABLE Category (
    CategoryId INT IDENTITY(1,1),
    CategoryName NVARCHAR(50) NOT NULL,
    -- Add other category-related fields as needed
    CONSTRAINT PK_Category_CategoryId PRIMARY KEY (CategoryId)
);

-- Product Table
CREATE TABLE Product (
    ProductId INT IDENTITY(1,1),
    ProductName NVARCHAR(100) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    Quantity INT NOT NULL,
    CategoryId INT,
    -- Add other product-related fields as needed
    CONSTRAINT PK_Product_ProductId PRIMARY KEY (ProductId),
    CONSTRAINT FK_Product_Category FOREIGN KEY (CategoryId) REFERENCES Category(CategoryId)
);

-- Address Table (Combining Shipping and Billing Addresses)
CREATE TABLE Address (
    AddressId INT IDENTITY(1,1),
    UserId INT,
    Street NVARCHAR(255) NOT NULL,
    City NVARCHAR(50) NOT NULL,
    State NVARCHAR(50) NOT NULL,
    ZipCode NVARCHAR(20) NOT NULL,
    IsShippingAddress BIT NOT NULL, -- Indicates whether it's a shipping address
    -- Add other address-related fields as needed
    CONSTRAINT PK_Address_AddressId PRIMARY KEY (AddressId),
    CONSTRAINT FK_Address_UserProfile FOREIGN KEY (UserId) REFERENCES UserProfile(UserId)
);


-- Orders Table
CREATE TABLE Orders (
    OrderId INT IDENTITY(1,1),
    UserId INT,
    OrderDate DATETIME NOT NULL,
    TotalAmount DECIMAL(10, 2) NOT NULL,
    -- Add other order-related fields as needed
    CONSTRAINT PK_Orders_OrderId PRIMARY KEY (OrderId),
    CONSTRAINT FK_Orders_UserProfile FOREIGN KEY (UserId) REFERENCES UserProfile(UserId)
);

-- OrderItem Table (to represent items in an order)
CREATE TABLE OrderItem (
    OrderItemId INT IDENTITY(1,1),
    OrderId INT,
    ProductId INT,
    Quantity INT NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    TotalCost DECIMAL(10, 2) NOT NULL, -- Added TotalCost column
    -- Add other order item-related fields as needed
    CONSTRAINT PK_OrderItem_OrderItemId PRIMARY KEY (OrderItemId),
    CONSTRAINT FK_OrderItem_Orders FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    CONSTRAINT FK_OrderItem_Product FOREIGN KEY (ProductId) REFERENCES Product(ProductId)
);

-- PaymentInformation Table
CREATE TABLE PaymentInformation (
    PaymentId INT IDENTITY(1,1),
    OrderId INT,
    PaymentAmount DECIMAL(10, 2) NOT NULL,
    PaymentDate DATETIME NOT NULL,
    PaymentMethod NVARCHAR(50), -- Store the payment method (e.g., "Credit Card", "PayPal", etc.)
    -- Add other payment-related fields as needed
    CONSTRAINT PK_PaymentInformation_PaymentId PRIMARY KEY (PaymentId),
    CONSTRAINT FK_PaymentInformation_Orders FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
);


-- OrderStatus Table
CREATE TABLE OrderStatus (
    StatusId INT IDENTITY(1,1),
    OrderId INT,
    StatusName NVARCHAR(50) NOT NULL,
    -- Add other status-related fields as needed
    CONSTRAINT PK_OrderStatus_StatusId PRIMARY KEY (StatusId),
    CONSTRAINT FK_OrderStatus_Orders FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
);

-- ProductReview Table
CREATE TABLE ProductReview (
    ReviewId INT IDENTITY(1,1),
    ProductId INT,
    UserId INT,
    Rating INT CHECK (Rating >= 1 AND Rating <= 5) NOT NULL,
    ReviewText NVARCHAR(MAX),
    ReviewDate DATETIME NOT NULL,
    -- Add other review-related fields as needed
    CONSTRAINT PK_ProductReview_ReviewId PRIMARY KEY (ReviewId),
    CONSTRAINT FK_ProductReview_Product FOREIGN KEY (ProductId) REFERENCES Product(ProductId),
    CONSTRAINT FK_ProductReview_UserProfile FOREIGN KEY (UserId) REFERENCES UserProfile(UserId)
);

-- ProductImage Table
CREATE TABLE ProductImage (
    ImageId INT IDENTITY(1,1),
    ProductId INT,
    ImageUrl NVARCHAR(255) NOT NULL,
    -- Add other image-related fields as needed
    CONSTRAINT PK_ProductImage_ImageId PRIMARY KEY (ImageId),
    CONSTRAINT FK_ProductImage_Product FOREIGN KEY (ProductId) REFERENCES Product(ProductId)
);

-- Coupon Table
CREATE TABLE Coupon (
    CouponId INT IDENTITY(1,1),
    CouponCode NVARCHAR(20) NOT NULL,
    DiscountAmount DECIMAL(10, 2) NOT NULL,
    ExpiryDate DATETIME NOT NULL,
    ProductId INT, -- Nullable, to indicate product-specific coupon
    CategoryId INT, -- Nullable, to indicate category-specific coupon
    -- Add other coupon-related fields as needed
    CONSTRAINT PK_Coupon_CouponId PRIMARY KEY (CouponId),
    CONSTRAINT FK_Coupon_Product FOREIGN KEY (ProductId) REFERENCES Product(ProductId),
    CONSTRAINT FK_Coupon_Category FOREIGN KEY (CategoryId) REFERENCES Category(CategoryId)
);

-- OrderCoupon Table
CREATE TABLE OrderCoupon (
    OrderCouponId INT IDENTITY(1,1),
    OrderId INT,
    CouponId INT,
    -- Add other fields as needed, such as DiscountAmount
    CONSTRAINT PK_OrderCoupon_OrderCouponId PRIMARY KEY (OrderCouponId),
    CONSTRAINT FK_OrderCoupon_Orders FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    CONSTRAINT FK_OrderCoupon_Coupon FOREIGN KEY (CouponId) REFERENCES Coupon(CouponId)
);


-- UserActivityLog Table
CREATE TABLE UserActivityLog (
    LogId INT IDENTITY(1,1),
    UserId INT,
    ActivityType NVARCHAR(50) NOT NULL,
    ActivityDescription NVARCHAR(MAX),
    LogDate DATETIME NOT NULL,
    -- Add other log-related fields as needed
    CONSTRAINT PK_UserActivityLog_LogId PRIMARY KEY (LogId),
    CONSTRAINT FK_UserActivityLog_UserProfile FOREIGN KEY (UserId) REFERENCES UserProfile(UserId)
);

-- Cart Table
CREATE TABLE Cart (
    CartId INT IDENTITY(1,1),
    UserId INT,
    ProductId INT,
    Quantity INT NOT NULL,
    -- Add other cart-related fields as needed
    CONSTRAINT PK_Cart_CartId PRIMARY KEY (CartId),
    CONSTRAINT FK_Cart_UserProfile FOREIGN KEY (UserId) REFERENCES UserProfile(UserId),
    CONSTRAINT FK_Cart_Product FOREIGN KEY (ProductId) REFERENCES Product(ProductId)
);

-- Wishlist Table
CREATE TABLE Wishlist (
    WishlistId INT IDENTITY(1,1),
    UserId INT,
    ProductId INT,
    -- Add other wishlist-related fields as needed
    CONSTRAINT PK_Wishlist_WishlistId PRIMARY KEY (WishlistId),
    CONSTRAINT FK_Wishlist_UserProfile FOREIGN KEY (UserId) REFERENCES UserProfile(UserId),
    CONSTRAINT FK_Wishlist_Product FOREIGN KEY (ProductId) REFERENCES Product(ProductId)
);


---- Create an INSTEAD OF INSERT trigger to enforce the unique condition
--CREATE TRIGGER trg_EnforceSingleShippingAddress
--ON Address
--INSTEAD OF INSERT
--AS
--BEGIN
--    IF EXISTS (
--        SELECT 1
--        FROM inserted i
--        JOIN Address a ON i.UserId = a.UserId AND i.IsShippingAddress = 1 AND a.IsShippingAddress = 1
--        WHERE i.AddressId IS NULL OR i.AddressId <> a.AddressId
--    )
--    BEGIN
--        RAISEERROR('Only one shipping address is allowed per user.', 16, 1);
--        ROLLBACK;
--        RETURN;
--    END;

--    INSERT INTO Address (AddressId, UserId, Street, City, State, ZipCode, IsShippingAddress)
--    SELECT AddressId, UserId, Street, City, State, ZipCode, IsShippingAddress
--    FROM inserted;
--END;

/*
--Here's a simplified example of how you might calculate the final order payment value:
-- Assume OrderId = 1 and the original order total is $100.00
DECLARE @OriginalOrderTotal DECIMAL(10, 2);
SET @OriginalOrderTotal = 100.00;

-- Calculate the total discount applied to the order based on applied coupons
DECLARE @TotalDiscount DECIMAL(10, 2);
SELECT @TotalDiscount = SUM(DiscountAmount)
FROM OrderCoupon
WHERE OrderId = 1;

-- Calculate the final order payment value
DECLARE @FinalOrderPayment DECIMAL(10, 2);
SET @FinalOrderPayment = @OriginalOrderTotal - @TotalDiscount;

-- Display the final order payment value
SELECT @FinalOrderPayment AS FinalOrderPayment;

*/

/*
-- Here's a suggested grouping of tables into microservices based on their functional relationships:
User Microservice:

UserProfile
UserRole
UserActivityLog
Address (User addresses, including shipping and billing addresses)

Product Microservice:

Category
Product
ProductImage
ProductReview

Order Microservice:

Orders
OrderItem
OrderStatus
PaymentInformation
OrderCoupon
Coupon

Cart and Wishlist Microservices:

Cart
Wishlist
*/

INSERT INTO UserProfile ( DisplayName, FirstName, LastName, Email, AdObjId)
VALUES
( 'JohnDoe', 'John', 'Doe', 'john.doe@example.com', '1a2b3c4d-5e6f-7g8h-9i10-11j12k13l14m'),
('JaneSmith', 'Jane', 'Smith', 'jane.smith@example.com', '2n3o4p5q-6r7s-8t9u-10v11w12x13y14')


-- Roles Table
INSERT INTO Roles (RoleName)
VALUES
('Admin'),
('ReadOnly'),
('Support');
-- Add more rows as needed

-- UserRole Table
INSERT INTO UserRole (RoleId, UserId)
VALUES
(1, 1), -- JohnDoe is an Admin
(2, 2), -- JaneSmith is a ReadOnly user
(3, 1); -- JohnDoe has Support role
-- Add more rows as needed

-- Address Table
INSERT INTO Address ( UserId, Street, City, State, ZipCode, IsShippingAddress)
VALUES
(1, '123 Main St', 'Cityville', 'Stateville', '12345', 1),
(1, '456 Oak St', 'Townsville', 'Stateville', '67890', 0),
(2, '789 Pine St', 'Villageville', 'Stateville', '54321', 1);
-- Add more rows as needed

-- UserActivityLog Table
INSERT INTO UserActivityLog ( UserId, ActivityType, ActivityDescription, LogDate)
VALUES
(1, 'Login', 'User logged in successfully', '2023-01-01T10:30:00'),
(2, 'UpdateProfile', 'User updated profile information', '2023-01-02T15:45:00'),
(1, 'Purchase', 'User made a purchase', '2023-01-03T08:15:00');

-- Insert statements for Category table
INSERT INTO Category (CategoryName)
VALUES ('Electronics');

INSERT INTO Category (CategoryName)
VALUES ('Clothing');

-- Add more categories as needed...

-- Insert statements for Product table
INSERT INTO Product (ProductName, Price, Quantity, CategoryId)
VALUES ('Smartphone', 499.99, 100, 1);

INSERT INTO Product (ProductName, Price, Quantity, CategoryId)
VALUES ('Laptop', 899.99, 50, 1);

INSERT INTO Product (ProductName, Price, Quantity, CategoryId)
VALUES ('T-Shirt', 19.99, 200, 2);

INSERT INTO Product (ProductName, Price, Quantity, CategoryId)
VALUES ('Jeans', 39.99, 100, 2);