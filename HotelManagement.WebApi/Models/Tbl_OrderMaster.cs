namespace HotelManagement.WebApi.Models
{
    public class Tbl_OrderMaster
    {
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public decimal CustomerMobile { get; set; }
        public decimal ItemQuantity { get; set; }
        public double TotalPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool Active { get; set; }

        public Tbl_MenuItem MenuItem { get; set; }
    }


    public class OrderMaster
    {
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public decimal CustomerMobile { get; set; }
        public decimal ItemQuantity { get; set; }
        public double TotalPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedById { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool Active { get; set; }
    }


}
