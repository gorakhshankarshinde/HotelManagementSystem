namespace HotelManagement.WebApi.Models
{
    public class UserWithType
    {
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public decimal UserContactNumber { get; set; }
        public string UserEmailAddress { get; set; }
        public int UserTypeId { get; set; }
        public string UserType { get; set; }
        public int UserCreatedBy { get; set; }
        public DateTime UserCreatedOn { get; set; }
        public int? UserUpdatedBy { get; set; }
        public DateTime? UserUpdatedOn { get; set; }
        public bool UserActive { get; set; }
        public bool UserTypeActive { get; set; }
    }
}
