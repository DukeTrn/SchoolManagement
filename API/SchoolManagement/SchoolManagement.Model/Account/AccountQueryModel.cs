using SchoolManagement.Common.Enum;

namespace SchoolManagement.Model.Account
{
    public class AccountQueryModel : PageQueryModel
    {
        public List<RoleType> Roles { get; set; } = new List<RoleType>();
    }
}
