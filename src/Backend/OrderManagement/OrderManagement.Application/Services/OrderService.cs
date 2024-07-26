using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Exceptions;
using OrderManagement.Infrastructure.Persistence;

namespace OrderManagement.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private const int STATUS_NEW_ORDER = 1;
        private const int ID_USER_CREATED = 1; // Just a value for the fake user
        private const int NEW_STATUS = 1;
        private const int PROCESSING_STATUS = 2;
        private const int COMPLETED_STATUS = 3;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(OrderCreateDto orderDto)
        {
            var shop = await _context.Shops.FirstOrDefaultAsync(s => s.Id == orderDto.ShopId);
            if (shop == null)
            {
                throw new NotFoundException("Shop not found");
            }

            foreach (var orderProductDto in orderDto.OrderProducts)
            {
                var product = await _context.Products.FindAsync(orderProductDto.ProductId);
                if (product == null)
                {
                    throw new NotFoundException($"Product with ID {orderProductDto.ProductId} not found");
                }
            }

            var order = new Order
            {
                ShopId = orderDto.ShopId,
                OrderDate = DateTime.UtcNow,
                StatusId = STATUS_NEW_ORDER,
                TotalAmount = orderDto.TotalAmount,
                idUserCreated = ID_USER_CREATED,
                OrderProducts = new List<OrderProduct>()
            };

            foreach (var orderProductDto in orderDto.OrderProducts)
            {
                order.OrderProducts.Add(new OrderProduct
                {
                    ProductId = orderProductDto.ProductId,
                    Quantity = orderProductDto.Quantity
                });
            }

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .Include(o => o.Status)
                .Include(o => o.Shop)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                throw new NotFoundException("Order not found");
            }

            var orderDto = new OrderDto
            {
                IdOrder = order.Id,
                IdShop = order.ShopId,
                NameShop = order.Shop.Name,
                DateOrder = order.OrderDate,
                StatusId = order.StatusId,
                StatusName = order.Status.Name,
                TotalAmount = order.TotalAmount,
                Products = order.OrderProducts.Select(op => new OrderProductDto
                {
                    ProductId = op.ProductId,
                    ProductName = op.Product.Name,
                    Quantity = op.Quantity,
                    Price = op.Product.Price
                }).ToList()
            };

            return orderDto;
        }

        public async Task<IEnumerable<OrderListDto>> GetOrderListAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Status)
                .Include(o => o.Shop)
                .Where(o => o.isEnabled)
                .ToListAsync();

            return orders.Select(o => new OrderListDto
            {
                IdOrder = o.Id,
                ShopName = o.Shop.Name,
                StatusId = o.StatusId,
                StatusName = o.Status.Name,
                TotalAmount = o.TotalAmount
                //TotalAmount = o.OrderProducts.Sum(op => op.Quantity * op.Product.Price)
            }).ToList();
        }

        public async Task<IEnumerable<OrderStatusDto>> GetOrderStatusListAsync()
        {
            var statuses = await _context.OrderStatuses.ToListAsync();
            return statuses.Select(s => new OrderStatusDto
            {
                IdStatus = s.Id,
                StatusName = s.Name
            }).ToList();
        }

        public async Task SoftDeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id) ??
                throw new NotFoundException("Order not found");

            order.isEnabled = false;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderStatusAsync(int idOrder, int statusId)
        {
            var order = await _context.Orders.FindAsync(idOrder) ??
                throw new NotFoundException("Order not found");

            _ = await _context.OrderStatuses.FindAsync(statusId) ??
                throw new NotFoundException("Status not found");

            order.StatusId = statusId;
            order.dateUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderToNextStatusAsync(int idOrder)
        {
            var order = await _context.Orders
                .Include(o => o.Status)
                .FirstOrDefaultAsync(o => o.Id == idOrder);

            if (order == null)
            {
                throw new NotFoundException("Order not found");
            }

            var currentStatusId = order.StatusId;

            int nextStatusId;
            switch (currentStatusId) 
            {
                case NEW_STATUS:
                    nextStatusId = PROCESSING_STATUS;
                    break;
                case PROCESSING_STATUS:
                    nextStatusId = COMPLETED_STATUS;
                    break;
                case COMPLETED_STATUS:
                    throw new ValidationException("Order is already completed.");
                default:
                    throw new ValidationException("Invalid order status.");
            }

            var nextStatus = await _context.OrderStatuses.FindAsync(nextStatusId) ??
                throw new NotFoundException("Next status not found");

            order.StatusId = nextStatusId;
            order.dateUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
