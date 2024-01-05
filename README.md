# E-Commerce Microservices Application
This is a simple e-commerce application built using microservices architecture. The application is designed to manage user profiles, products, orders, and related functionalities. Each microservice corresponds to a specific domain, facilitating modular development and scalability.

## Table of Contents

### Microservices
1. User Microservice
2. Product Microservice
3. Order Microservice
4. Cart and Wishlist Microservices

### Database Schema

#### Microservices
1. User Microservice
Manages user profiles, roles, and activity logs.
Handles user addresses, including shipping and billing addresses.
2. Product Microservice
Manages product categories, products, images, and reviews.
3. Order Microservice
Manages orders, order items, order status, payments, and coupons.
Supports the application of coupons to orders.
4. Cart and Wishlist Microservices
Manages user shopping carts and wishlists.

#### Database Schema
The application uses a shared database with the following tables:

1. UserProfile
2. UserRole
3. UserActivityLog
4. Address
5. Category
6. Product
7. ProductImage
8. ProductReview
9. Orders
10. OrderItem
11. PaymentInformation
12. OrderStatus
13. Coupon
14. OrderCoupon
15. UserActivityLog
16. Cart
17. Wishlist

Refer to the database script for detailed schema and constraints.

### Database Connection

```
"Data Source=[servername];Initial Catalog=[database_name];Integrated Security=SSPI; MultipleActiveResultSets=true;"
```
when using localdb use "(LocalDb)\\MSSQLLocalDB" as server name

To Scaffold database as model to local project use below command.

```
dotnet ef dbcontext scaffold "Data Source=[servername];Initial Catalog=LearnSmartDB;Integrated Security=SSPI; MultipleActiveResultSets=true;" Microsoft.EntityFrameworkCore.SqlServer -o Entities -d
```

## Here's a suggested grouping of tables into microservices based on their functional relationships:

#### User Microservice:

This Microservice is deployed to Azure and can be accessed here https://lsc-ecommerce-userprofile.azurewebsites.net/swagger/index.html

- UserProfile
- UserRole
- UserActivityLog
- Address (User addresses, including shipping and billing addresses)

#### Product Microservice:

- Category
- Product
- ProductImage
- ProductReview

#### Order Microservice:

- Orders
- OrderItem
- OrderStatus
- PaymentInformation
- OrderCoupon
- Coupon

#### Cart and Wishlist Microservices:

- Cart
- Wishlist

## Database Scripts
Database script can be found at root level of this repository E.g. LearnSmartDB_DB_Script.sql