namespace HotelManagement.WebApi.Models
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public string MenuItemType { get; set; }
        public decimal Price { get; set; }
    }
}
