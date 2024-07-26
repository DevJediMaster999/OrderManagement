namespace OrderManagement.Application.DTOs
{
    public class OrderDto
    {
        public int IdOrder { get; set; }
        public int IdShop { get; set; }
        public string NameShop { get; set; }
        public List<OrderProductDto> Products { get; set; }
        public DateTime DateOrder { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
