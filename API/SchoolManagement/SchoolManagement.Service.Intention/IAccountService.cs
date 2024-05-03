using SchoolManagement.Entity;
using SchoolManagement.Model;

namespace SchoolManagement.Service.Intention
{
    public interface IAccountService
    {
        ValueTask<AccountEntity> CreateAccount(AccountAddModel model);
    }
}