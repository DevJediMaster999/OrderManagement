using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Services;
using OrderManagement.Domain.Entities;
using OrderManagement.Infrastructure.Persistence;

namespace OrderManagement.Tests
{
    public class OrderServiceTests
    {
        private readonly AppDbContext _context;
        private readonly IOrderService _orderService;

        public OrderServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "OrderManagementTest")
                .Options;

            _context = new AppDbContext(options);
            _orderService = new OrderService(_context);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            if (!_context.Shops.Any())
            {
                var shop = new Shop { Id = 1, Name = "Test Shop", Phone="222-222-2222", Email = "shop@test.com", idUserCreated = 1 };
                _context.Shops.Add(shop);
            }

            if (!_context.OrderStatuses.Any())
            {
                var statusNew = new OrderStatus { Id = 1, Name = "New" };
                var statusProcessing = new OrderStatus { Id = 2, Name = "Processing" };
                var statusCompleted = new OrderStatus { Id = 3, Name = "Completed" };
                _context.OrderStatuses.AddRange(statusNew, statusProcessing, statusCompleted);
            }

            if (!_context.Products.Any())
            {
                var product = new Product { Id = 1, Name = "Test Product", Price = 10.0M, idUserCreated = 1, Description="product" };
                _context.Products.Add(product);
            }

            _context.SaveChanges();
        }

        private void ClearDatabase()
        {
            _context.Orders.RemoveRange(_context.Orders);
            _context.SaveChanges();
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldCreateOrder()
        {
            ClearDatabase();

            // Arrange
            var orderDto = new OrderCreateDto
            {
                ShopId = 1,
                TotalAmount = 10.0M,
                OrderProducts = new List<OrderProductCreateDto>
                {
                    new OrderProductCreateDto { ProductId = 1, Quantity = 1 }
                }
            };

            // Act
            var result = await _orderService.CreateOrderAsync(orderDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.ShopId);
            Assert.Equal(10.0M, result.TotalAmount);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnOrderDetails()
        {
            ClearDatabase();

            // Arrange
            var order = new Order
            {
                ShopId = 1,
                StatusId = 1,
                OrderDate = DateTime.Now,
                TotalAmount = 10.0M,
                OrderProducts = new List<OrderProduct>
                {
                    new OrderProduct { ProductId = 1, Quantity = 1 }
                }
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Act
            var result = await _orderService.GetOrderByIdAsync(order.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.Id, result.IdOrder);
            Assert.Equal(order.ShopId, result.IdShop);
            Assert.Equal(order.TotalAmount, result.TotalAmount);
            Assert.Single(result.Products);
        }

        [Fact]
        public async Task GetOrderListAsync_ShouldReturnAllOrders()
        {
            ClearDatabase();

            // Arrange
            var order1 = new Order
            {
                ShopId = 1,
                StatusId = 1,
                OrderDate = DateTime.Now,
                TotalAmount = 10.0M,
                OrderProducts = new List<OrderProduct>
                {
                    new OrderProduct { ProductId = 1, Quantity = 1 }
                }
            };

            var order2 = new Order
            {
                ShopId = 1,
                StatusId = 1,
                OrderDate = DateTime.Now,
                TotalAmount = 20.0M,
                OrderProducts = new List<OrderProduct>
                {
                    new OrderProduct { ProductId = 1, Quantity = 2 }
                }
            };

            _context.Orders.AddRange(order1, order2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _orderService.GetOrderListAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task UpdateOrderStatusAsync_ShouldUpdateStatus()
        {
            ClearDatabase();

            // Arrange
            var order = new Order
            {
                ShopId = 1,
                StatusId = 1,
                OrderDate = DateTime.Now,
                TotalAmount = 10.0M,
                OrderProducts = new List<OrderProduct>
                {
                    new OrderProduct { ProductId = 1, Quantity = 1 }
                }
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Act
            await _orderService.UpdateOrderStatusAsync(order.Id, 2);

            // Assert
            var updatedOrder = await _context.Orders.FindAsync(order.Id);
            Assert.Equal(2, updatedOrder.StatusId);
        }

        [Fact]
        public async Task UpdateOrderToNextStatusAsync_ShouldUpdateToNextStatus()
        {
            ClearDatabase();

            // Arrange
            var order = new Order
            {
                ShopId = 1,
                StatusId = 1,
                OrderDate = DateTime.Now,
                TotalAmount = 10.0M,
                OrderProducts = new List<OrderProduct>
                {
                    new OrderProduct { ProductId = 1, Quantity = 1 }
                }
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Act
            await _orderService.UpdateOrderToNextStatusAsync(order.Id);

            // Assert
            var updatedOrder = await _context.Orders.FindAsync(order.Id);
            Assert.Equal(2, updatedOrder.StatusId);
        }

        [Fact]
        public async Task SoftDeleteOrder_ShouldDisableOrder()
        {
            ClearDatabase();

            // Arrange
            var order = new Order
            {
                ShopId = 1,
                StatusId = 1,
                OrderDate = DateTime.Now,
                TotalAmount = 10.0M,
                isEnabled = true,
                OrderProducts = new List<OrderProduct>
                {
                    new OrderProduct { ProductId = 1, Quantity = 1 }
                }
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Act
            await _orderService.SoftDeleteOrder(order.Id);

            // Assert
            var deletedOrder = await _context.Orders.FindAsync(order.Id);
            Assert.False(deletedOrder.isEnabled);
        }
    }
}
