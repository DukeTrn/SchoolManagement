using SchoolManagement.Common.Enum;

namespace SchoolManagement.Model
{
    public class AccountAddModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public RoleType Role { get; set; }
    }
}
