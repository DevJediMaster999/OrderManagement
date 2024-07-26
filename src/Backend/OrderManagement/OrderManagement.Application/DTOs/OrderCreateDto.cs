using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Application.DTOs
{
    public class OrderCreateDto
    {
        [Required]
        public int ShopId { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public ICollection<OrderProductCreateDto> OrderProducts { get; set; }
    }
}
