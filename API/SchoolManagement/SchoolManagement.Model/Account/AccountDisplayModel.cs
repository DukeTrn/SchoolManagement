using SchoolManagement.Common.Enum;

namespace SchoolManagement.Model
{
    public class AccountDisplayModel
    {
        public Guid AccountId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; } = string.Empty; // enum 
    }
}
