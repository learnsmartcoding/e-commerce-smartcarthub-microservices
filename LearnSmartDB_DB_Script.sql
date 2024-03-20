
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'SmartCartHub')
BEGIN
    -- Create the database
    CREATE DATABASE SmartCartHub;
END
ELSE
BEGIN
   DROP DATABASE SmartCartHub;
END

Go
use SmartCartHub
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
    ProductDescription NVARCHAR(4000) NOT NULL,
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

--Contact Us table
CREATE TABLE ContactUs (
    ContactUsId INT IDENTITY(1,1),
    UserName NVARCHAR(100) NOT NULL,
	UserEmail NVARCHAR(100) NOT NULL,
	MessageDetail NVARCHAR(2000) NOT NULL,    
    CONSTRAINT PK_ContactUs_ContactUsId PRIMARY KEY (ContactUsId)    
);


-- To drop all tables without deleting database
--DROP TABLE IF EXISTS UserProfile
--DROP TABLE IF EXISTS Roles
--DROP TABLE IF EXISTS UserRole
--DROP TABLE IF EXISTS Category
--DROP TABLE IF EXISTS Product
--DROP TABLE IF EXISTS Address
--DROP TABLE IF EXISTS Orders
--DROP TABLE IF EXISTS OrderItem
--DROP TABLE IF EXISTS PaymentInformation
--DROP TABLE IF EXISTS OrderStatus
--DROP TABLE IF EXISTS ProductReview
--DROP TABLE IF EXISTS ProductImage
--DROP TABLE IF EXISTS Coupon
--DROP TABLE IF EXISTS OrderCoupon
--DROP TABLE IF EXISTS UserActivityLog
--DROP TABLE IF EXISTS Cart
--DROP TABLE IF EXISTS Wishlist
--DROP TABLE IF EXISTS ContactUs

-- ends here
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


-- Roles Table
INSERT INTO Roles (RoleName)
VALUES
('Admin'),
('ReadOnly'),
('Support'),
('User');

-- Insert statements for Category table

INSERT INTO Category (CategoryName)
VALUES 
('Electronics'),
('Clothing'),
('Mobile');
