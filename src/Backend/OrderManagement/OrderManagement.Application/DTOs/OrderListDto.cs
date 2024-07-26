namespace OrderManagement.Application.DTOs
{
    public class OrderListDto
    {
        public int IdOrder { get; set; }
        public string ShopName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
