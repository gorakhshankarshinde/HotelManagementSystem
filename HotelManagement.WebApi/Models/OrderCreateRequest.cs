namespace HotelManagement.WebApi.Models
{
    public class OrderCreateRequest
    {
        public int MenuItemId { get; set; }
        public long CustomerMobile { get; set; }
        public int ItemQuantity { get; set; }
        public double TotalPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool Active { get; set; } = true;
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
    }
}
