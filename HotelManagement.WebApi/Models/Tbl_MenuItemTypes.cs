namespace HotelManagement.WebApi.Models
{
    public class Tbl_MenuItemTypes
    {
        public int MenuItemTypeId { get; set; }
        public string MenuItemType { get; set; }
        public bool Active { get; set; }

        // Navigation property
        public ICollection<Tbl_MenuItem> MenuItems { get; set; }
    }

}
