namespace HotelManagement.WebApi.Models
{
    public class Tbl_UserType
    {
        public int UserTypeId { get; set; }
        public string UserType { get; set; }
        public bool Active { get; set; }

        // Navigation property
        public ICollection<Tbl_User> Users { get; set; }
    }

}
