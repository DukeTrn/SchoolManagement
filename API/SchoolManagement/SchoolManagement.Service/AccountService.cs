using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Common.Enum;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Database;
using SchoolManagement.Entity;
using SchoolManagement.Model;
using SchoolManagement.Model.Account;
using SchoolManagement.Service.Intention;
using SchoolManagement.Service.Intention.Data;

namespace SchoolManagement.Service
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly SchoolManagementDbContext _context;
        private readonly IEntityFilterService<AccountEntity> _filterBuilder;

        public AccountService(ILogger<AccountService> logger, 
            SchoolManagementDbContext context,
            IEntityFilterService<AccountEntity> filterBuilder)
        {
            _logger = logger;
            _context = context;
            _filterBuilder = filterBuilder;
        }

        /// <summary>
        /// Get list of all accounts with pagination
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public async ValueTask<PaginationModel<AccountDisplayModel>> GetAllAccounts(AccountQueryModel queryModel)
        {
            try
            {
                _logger.LogInformation("Start to get list accounts.");
                int pageNumber = queryModel.PageNumber != null && queryModel.PageNumber.Value > 0 ? queryModel.PageNumber.Value : 1;
                int pageSize = queryModel.PageSize != null && queryModel.PageSize.Value > 0 ? queryModel.PageSize.Value : 10;
                FilterModel filter = new();
                var query = _context.AccountEntities.AsQueryable();
                int totalCount = await query.CountAsync();
               
                #region Search
                if (!string.IsNullOrEmpty(queryModel.SearchValue))
                {
                    _logger.LogInformation("Add Search Value: {SearchValue}", queryModel.SearchValue);
                    var searchFilter = BuildSearchFilter(queryModel.SearchValue,
                        nameof(AccountEntity.UserName));
                    filter.Or.AddRange(searchFilter);
                }
                if (!string.IsNullOrEmpty(queryModel.SearchValue))
                {
                    query = query.Where(a => (a.Student != null && a.Student.FullName.Contains(queryModel.SearchValue)) ||
                                              (a.Teacher != null && a.Teacher.FullName.Contains(queryModel.SearchValue)));
                }
                #endregion

                query = _filterBuilder.BuildFilterQuery(query, filter);

                var accounts = await query
                    .Include(a => a.Student)
                    .Include(a => a.Teacher)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var accountDisplayModels = accounts.Select(a => new AccountDisplayModel
                {
                    AccountId = a.AccountId,
                    FullName = a.Student != null ? a.Student.FullName : a.Teacher != null ? a.Teacher.FullName : string.Empty,
                    UserName = a.UserName,
                    Password = a.Password,
                    CreatedAt = a.CreatedAt,
                    ModifiedAt = a.ModifiedAt,
                    IsActive = a.IsActive,
                    Role = TranslateStatus(a.Role)
                }).ToList();
                return new PaginationModel<AccountDisplayModel>
                {
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    DataList = accountDisplayModels
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while getting list of all accounts. Error: {ex}", ex);
                throw;
            }
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
                _logger.LogError("An error occured while creating new acoount. Error: {ex}", ex.Message);
                throw;
            }
        }

        public async Task ChangePasswordAsync(Guid accountId, ChangePasswordModel model)
        {
            try
            {
                _logger.LogInformation($"Start to change password for account with ID {accountId}.");

                var account = await _context.AccountEntities.FindAsync(accountId);

                if (account == null)
                {
                    _logger.LogWarning($"Account with ID {accountId} not found.");
                    throw new NotFoundException($"Account with ID {accountId} not found.");
                }

                // Kiểm tra mật khẩu cũ
                if (!BCrypt.Net.BCrypt.Verify(model.OldPassword, account.Password))
                {
                    _logger.LogWarning($"Old password provided is incorrect for account with ID {accountId}.");
                    throw new UnauthorizedAccessException("Old password provided is incorrect.");
                }

                // Mật khẩu mới phải khác mật khẩu cũ
                if (model.NewPassword == model.OldPassword)
                {
                    _logger.LogWarning($"New password must be different from old password for account with ID {accountId}.");
                    throw new ArgumentException("New password must be different from old password.");
                }

                // Xác nhận mật khẩu mới
                if (model.NewPassword != model.ConfirmPassword)
                {
                    _logger.LogWarning($"New password and confirm password do not match for account with ID {accountId}.");
                    throw new ArgumentException("New password and confirm password do not match.");
                }

                // Mã hóa mật khẩu mới
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
                account.Password = hashedPassword;
                account.ModifiedAt = DateTime.UtcNow;

                _context.AccountEntities.Update(account);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Successfully changed password for account with ID {accountId}.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while changing password for account with ID {accountId}. Error: {ex.Message}");
                throw;
            }
        }

        //public async Task<string> GeneratePasswordResetTokenAsync(Guid accountId)
        //{
        //    var account = await _context.AccountEntities.FindAsync(accountId);
        //    if (account == null)
        //    {
        //        throw new NotFoundException($"Account with ID {accountId} not found.");
        //    }

        //    var token = Guid.NewGuid().ToString(); // Bạn có thể sử dụng bất kỳ logic nào để tạo token
        //    account.ResetToken = token;
        //    account.ResetTokenExpiry = DateTime.UtcNow.AddHours(1); // Token hết hạn sau 1 giờ

        //    await _context.SaveChangesAsync();

        //    return token;
        //}

        //public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        //{
        //    var account = await _context.AccountEntities.SingleOrDefaultAsync(a => a.ResetToken == token && a.ResetTokenExpiry > DateTime.UtcNow);
        //    if (account == null)
        //    {
        //        return false;
        //    }

        //    account.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
        //    account.ModifiedAt = DateTime.UtcNow;
        //    account.ResetToken = null; // Xóa token sau khi sử dụng
        //    account.ResetTokenExpiry = null;

        //    _context.AccountEntities.Update(account);
        //    await _context.SaveChangesAsync();

        //    return true;
        //}

        public async Task<AccountEntity> ValidateAccountAsync(string username, string password)
        {
            var account = await _context.AccountEntities.SingleOrDefaultAsync(a => a.UserName == username);

            if (account == null || !BCrypt.Net.BCrypt.Verify(password, account.Password))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            return account;
        }

        /// <summary>
        /// Search function
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        private static List<FilterOperatorItemModel> BuildSearchFilter(string searchKey, params string[] properties)
        {
            List<FilterOperatorItemModel> filters = new();
            foreach (var prop in properties)
            {
                filters.Add(new FilterOperatorItemModel()
                {
                    KeyName = prop,
                    Values = new List<string> { searchKey },
                    KeyType = typeof(string),
                    Operator = FilterOperator.Contains
                });
            }
            return filters;
        }

        // Translate StatusType enum
        private static string TranslateStatus(RoleType status)
        {
            switch (status)
            {
                case RoleType.Admin:
                    return "Admin";
                case RoleType.HomeroomTeacher:
                    return "GVCN";
                case RoleType.Teacher:
                    return "Giáo viên";
                case RoleType.Student:
                    return "Học sinh";
                default:
                    return string.Empty;
            }
        }
    }
}
