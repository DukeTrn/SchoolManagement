using SchoolManagement.Entity;
using SchoolManagement.Model;
using SchoolManagement.Model.Account;

namespace SchoolManagement.Service.Intention
{
    public interface IAccountService
    {
        ValueTask<PaginationModel<AccountDisplayModel>> GetAllAccounts(AccountQueryModel queryModel);
        ValueTask<AccountEntity> CreateAccount(AccountAddModel model);
    }
}