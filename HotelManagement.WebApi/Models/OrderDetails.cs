namespace HotelManagement.WebApi.Models
{
    public class OrderDetails
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public long CustomerMobile { get; set; }
        public int ItemQuantity { get; set; }
        public double TotalPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool OrderActive { get; set; }

        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public double MenuItemPrice { get; set; }
        public bool MenuItemActive { get; set; }

        public int MenuItemTypeId { get; set; }
        public string MenuItemType { get; set; }
        public bool MenuItemTypeActive { get; set; }
    }
}
