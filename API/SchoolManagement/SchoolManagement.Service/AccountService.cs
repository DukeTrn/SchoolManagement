using Microsoft.Extensions.Logging;
using SchoolManagement.Database;
using SchoolManagement.Entity;
using SchoolManagement.Model;
using SchoolManagement.Service.Intention;

namespace SchoolManagement.Service
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly SchoolManagementDbContext _context;

        public AccountService(ILogger<AccountService> logger, SchoolManagementDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Create new account
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async ValueTask<AccountEntity> CreateAccount(AccountAddModel model)
        {
            try
            {
                _logger.LogInformation("Start to create new account.");
                // Mã hóa mật khẩu trước khi lưu vào cơ sở dữ liệu
                // thuật toán mã hóa BlowFish
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                var newAccount = new AccountEntity()
                {
                    UserName = model.UserName,
                    Password = hashedPassword,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = null,
                    IsActive = true,
                    Role = model.Role,
                };
                _context.AccountEntities.Add(newAccount);
                await _context.SaveChangesAsync();

                return newAccount;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while creating new acoount. Error: {ex}", ex);
                throw;
            }
        }

    }
}
