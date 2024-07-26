namespace OrderManagement.Domain.Entities
{
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool isEnabled { get; set; } = true;
        public DateTime dateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? dateUpdated { get; set; }
        public int idUserCreated { get; set; }
        public int? idUserUpdated { get; set; }

        public ICollection<ShopProduct> ShopProducts { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
