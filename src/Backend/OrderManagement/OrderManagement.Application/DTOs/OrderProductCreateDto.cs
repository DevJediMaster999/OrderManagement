using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Application.DTOs
{
    public class OrderProductCreateDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
