ORDER MANAGEMENT APPLICATION

OVERVIEW
	This application manages orders, allowing users to view, update, and create orders. Key features include:

	-Viewing a list of all orders with detailed information.
	-Updating order status to the next status or selecting a specific status.
	-Creating new orders by selecting a shop and choosing products from that shop.

FEATURES

Order Management:
View Orders: List of all orders with details like Order ID, Shop Name, Status, and Total Amount. Click on an Icon (i) to see detailed information about order.
Update Status: Update the order status using a dropdown menu or "Next Status" button.
Create Order: Select a shop and choose products available in that shop.

FUTURE ENHANCEMENTS:
	-Display only active products.
	-Track and display completion or cancellation time of orders.
	-Implement email notifications for order updates.
	-Authentication and authorization.
	-Adding roles.
	-Adding jobs to see changes in real time - if needed.
	-Mail notifications 
	-Responsive layout
	-Ability to choose quantitity of products
	-Admin panel
	-Redesign order list


TECHNOLOGIES:

	 BACK-END:
		-.NET 8
		-Entity Framework Core
		-Microservice architecture
		-Unit tests XUnit

	FRONT-END
		-Vue 2
		-Vuetify
		-Axios 
		-Vuex 
		
HOW TO RUN APP:
 1. Run script Database\PRE__CREATE_INSERT_SCRIPT.sql to prepare database 
 2. Open solution src\Backend\OrderManagement\OrderManagement.sln
 3. Change connection string in the appsettings.json file in the OrderManagement.API project // Need to <YOUR_SOURCE> with your data
 4. Build and run // can test using swagger
 5. In VsCode open the folder: src\UI\order-management-ui
 6. In terminal run command npm-install to download need modules
 8. Run with command: npm run serve
 

Created by Kobylynskyi Vadym