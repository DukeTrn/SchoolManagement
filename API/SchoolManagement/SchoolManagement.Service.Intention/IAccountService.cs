using SchoolManagement.Entity;
using SchoolManagement.Model;
using SchoolManagement.Model.Account;

namespace SchoolManagement.Service.Intention
{
    public interface IAccountService
    {
        ValueTask<PaginationModel<AccountDisplayModel>> GetAllAccounts(AccountQueryModel queryModel);
        ValueTask<AccountEntity> CreateAccount(AccountAddModel model);
        ValueTask<AccountEntity> CreateAccountAutomatically(AccountAutomaticallyAddModel model);
        Task ChangePasswordAsync(Guid accountId, ChangePasswordModel model);
        Task<bool> DeleteAccountAsync(Guid accountId);
        Task<AccountEntity> ValidateAccountAsync(string username, string password);
        //Task<string> GeneratePasswordResetTokenAsync(Guid accountId);
        //Task<bool> ResetPasswordAsync(string token, string newPassword);
    }
}