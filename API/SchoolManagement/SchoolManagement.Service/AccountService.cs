using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Common.Enum;
using SchoolManagement.Database;
using SchoolManagement.Entity;
using SchoolManagement.Model;
using SchoolManagement.Model.Account;
using SchoolManagement.Service.Intention;
using SchoolManagement.Service.Intention.Data;
using SchoolManagement.Share;

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
                _logger.LogError("An error occured while creating new acoount. Error: {ex}", ex);
                throw;
            }
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
