Project Requirments:
https://github.com/2011-nov02-net/trainer-code/wiki/Project-1-requirements

Store Web App

This is my ASP.NET Core application that allows for users to view and order products from a store location with correct removal of stock from the location.  It also allows for viewing of all orders by a particular customer and all orders placed at a particular store.  Lastly, it all allows for the admin to add stock to existing products and add new products to store inventories.

Technologies Used:

- ASP.NET Core 5.0
- Entity Framework Core 5.0
- SQL Server
- Azure Cloud Services
  - App Service
  - SQL Server
- Xunit 2.4
- Moq 4.15
- Azure DevOps Pipeline
- SonarCloud
- BootStrap 4.5

Features:

- Customers can view all previous orders
- Customers can view all inventory at a given store
- Customer can add multiple products to order before placing

- Admin can view all orders at a given store
- Add product and inventory to a store

To-do list:

- Allow authentication to separate Admin/Customer features
- Allow orders to be placed at multiple stores
- Sort orders by date or amount

Getting Started:

- git clone https://github.com/2011-nov02-net/david-project1/
- Set up database with given sql file.
- Seed database with given insert files.
- Set up connection string in the user.secrets to your own database instance connection

In Visual Studio
  - Make sure Store.WebApp is current Startup Project
  - Click start.

On command line
  - navigate to Store.WebApp
  - dotnet run

Usage:

- Customer usage
  - Click on customer tab
  - Create new customer with New Customer link
  - Or select customer from list
  - Can View all customers orders by selecting details
  - Navigate to Store
  - Select Store to order from
  - Once both Store and Customer are selected, Place Order tab appears in Nav bar
  - In place order screen, select item from drop down, add quantity desires and click add to order
  - When all products that are desired in list, click place order to finalize the order

- Admin usage
  - Click on Store tab
  - Click on details for the store you wish to view
  - From there you can view all orders placed at that store and the amounts
  - From details page, you can view inventory at the current store
  - You can add inventory to existing items or add new items to the store

License
This project uses the following license: MIT.
