using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public int StatusId { get; set; }
        public OrderStatus Status { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        public bool isEnabled { get; set; } = true;
        public DateTime dateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? dateUpdated { get; set; }
        public int idUserCreated { get; set; }
        public int? idUserUpdated { get; set; }
        [Required]
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
