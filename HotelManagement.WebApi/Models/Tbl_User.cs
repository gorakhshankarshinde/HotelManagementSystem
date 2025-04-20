namespace HotelManagement.WebApi.Models
{
    public class Tbl_User
    {
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public decimal UserContactNumber { get; set; }
        public string UserEmailAddress { get; set; }
        public int UserTypeId { get; set; }
        public int? UserCreatedBy { get; set; }
        public DateTime UserCreatedOn { get; set; }
        public int? UserUpdatedBy { get; set; }
        public DateTime? UserUpdatedOn { get; set; }
        public bool Active { get; set; }

        // Navigation property
        public Tbl_UserType UserType { get; set; }
    }

}
