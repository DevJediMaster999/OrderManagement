using OrderManagement.Application.DTOs;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(OrderCreateDto orderDto);
        Task<OrderDto> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderListDto>> GetOrderListAsync();
        Task UpdateOrderStatusAsync(int idOrder, int statusId);
        Task UpdateOrderToNextStatusAsync(int idOrder);
        Task<IEnumerable<OrderStatusDto>> GetOrderStatusListAsync();
        Task SoftDeleteOrder(int id);
    }
}
