using System.ComponentModel.DataAnnotations;

namespace HotelManagement.WebApi.Models
{
    public class Tbl_MenuItem
    {
        public int MenuItemId { get; set; }  // Primary key
        public string MenuItemName { get; set; }
        public int MenuItemTypeId { get; set; }
        public double Price { get; set; }
        public bool Active { get; set; }
    }



}
