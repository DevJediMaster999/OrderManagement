using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;

namespace OrderManagement.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderListDto>>> GetOrderList()
        {
            var orders = await _orderService.GetOrderListAsync();
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderCreateDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _orderService.CreateOrderAsync(orderDto);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto updateOrderStatusDto)
        {
            await _orderService.UpdateOrderStatusAsync(id, updateOrderStatusDto.IdStatus);
            return Ok();
        }

        [HttpPatch("{idOrder}/next-status")]
        public async Task<IActionResult> UpdateOrderToNextStatus(int idOrder)
        {
            await _orderService.UpdateOrderToNextStatusAsync(idOrder);
            return Ok();
        }

        [HttpGet("status-list")]
        public async Task<ActionResult<IEnumerable<OrderStatusDto>>> GetOrderStatuses()
        {
            var statuses = await _orderService.GetOrderStatusListAsync();
            return Ok(statuses);
        }

        [HttpPatch("{id}/disable")]
        public async Task<IActionResult> DisableOrder(int id)
        {
            await _orderService.SoftDeleteOrder(id);
            return Ok();
        }
    }
}
