namespace OrderManagement.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool isEnabled { get; set; } = true;
        public DateTime dateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? dateUpdated { get; set; }
        public int idUserCreated { get; set; }
        public int? idUserUpdated { get; set; }

        public ICollection<ShopProduct> ShopProducts { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
