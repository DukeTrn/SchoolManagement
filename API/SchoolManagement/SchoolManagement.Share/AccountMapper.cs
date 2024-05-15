using SchoolManagement.Entity;
using SchoolManagement.Model;

namespace SchoolManagement.Share
{
    public static class AccountMapper
    {
        public static AccountDisplayModel ToModel(this AccountEntity e) => new()
        {
            AccountId = e.AccountId,
            UserName = e.UserName,
            Password = e.Password,
            CreatedAt = e.CreatedAt,
            ModifiedAt = e.ModifiedAt,
            IsActive = e.IsActive,
            //Role = e.Role,
        };
    }
}
