using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Common.Enum;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Common.Extensions;
using SchoolManagement.Database;
using SchoolManagement.Entity;
using SchoolManagement.Model;
using SchoolManagement.Service.Intention;
using SchoolManagement.Service.Intention.Data;
using SchoolManagement.Share;
using System.Data;

namespace SchoolManagement.Service
{
    public class TeacherService : ITeacherService
    {
        private readonly ILogger<TeacherService> _logger;
        private readonly SchoolManagementDbContext _context;
        private readonly IEntityFilterService<TeacherEntity> _filterBuilder;
        private readonly ICloudinaryService _cloudinary;
        private readonly IAccountService _account;

        public TeacherService(ILogger<TeacherService> logger,
            IEntityFilterService<TeacherEntity> filterBuilder,
            SchoolManagementDbContext context,
            ICloudinaryService cloudinary,
            IAccountService account)
        {
            _logger = logger;
            _filterBuilder = filterBuilder;
            _context = context;
            _cloudinary = cloudinary;
            _account = account;
        }

        #region List teachers
        /// <summary>
        /// Get list of all teachers with pagination
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public async ValueTask<PaginationModel<TeacherDisplayModel>> GetAllTeachers(TeacherQueryModel queryModel)
        {
            try
            {
                _logger.LogInformation("Start to get list teachers.");
                int pageNumber = queryModel.PageNumber != null && queryModel.PageNumber.Value > 0 ? queryModel.PageNumber.Value : 1;
                int pageSize = queryModel.PageSize != null && queryModel.PageSize.Value > 0 ? queryModel.PageSize.Value : 10;

                FilterModel filter = new();
                var query = _context.TeacherEntities.AsQueryable();

                #region Search
                if (!string.IsNullOrEmpty(queryModel.SearchValue))
                {
                    _logger.LogInformation("Add Search Value: {SearchValue}", queryModel.SearchValue);
                    var searchFilter = BuildSearchFilter(queryModel.SearchValue,
                        nameof(TeacherEntity.TeacherId),
                        nameof(TeacherEntity.FullName),
                        nameof(TeacherEntity.Gender),
                        nameof(TeacherEntity.Email),
                        nameof(TeacherEntity.Level),
                        nameof(TeacherEntity.PhoneNumber));
                    filter.Or.AddRange(searchFilter);
                }
                #endregion

                #region Status Filter
                if (queryModel.Status.Count > 0)
                {
                    _logger.LogInformation("Add Status condition: {Status}", queryModel.Status.ToString());
                    filter.AddAnd((TeacherEntity entity) => entity.Status, queryModel.Status);
                }
                #endregion

                query = _filterBuilder.BuildFilterQuery(query, filter);

                var paginatedData = await query
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
                return new PaginationModel<TeacherDisplayModel>
                {
                    TotalCount = query.Count(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    DataList = from item in paginatedData
                               select item.ToModel()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while getting list of all teachers. Error: {ex}", ex);
                throw;
            }
        }

        /// <summary>
        /// Get list of all teachers in 1 department with pagination
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public async ValueTask<PaginationModel<TeacherDisplayModel>> GetAllTeachers(string departmentId, TeacherQueryModel queryModel)
        {
            try
            {
                _logger.LogInformation("Start to get list teachers.");
                int pageNumber = queryModel.PageNumber != null && queryModel.PageNumber.Value > 0 ? queryModel.PageNumber.Value : 1;
                int pageSize = queryModel.PageSize != null && queryModel.PageSize.Value > 0 ? queryModel.PageSize.Value : 10;

                FilterModel filter = new();
                var query = _context.TeacherEntities.AsQueryable();
                query = query.Where(e => e.DepartmentId == departmentId);

                #region Search
                if (!string.IsNullOrEmpty(queryModel.SearchValue))
                {
                    _logger.LogInformation("Add Search Value: {SearchValue}", queryModel.SearchValue);
                    var searchFilter = BuildSearchFilter(queryModel.SearchValue,
                        nameof(TeacherEntity.TeacherId),
                        nameof(TeacherEntity.FullName),
                        nameof(TeacherEntity.Gender),
                        nameof(TeacherEntity.Email),
                        nameof(TeacherEntity.Level),
                        nameof(TeacherEntity.PhoneNumber));
                    filter.Or.AddRange(searchFilter);
                }
                #endregion

                #region Status Filter
                if (queryModel.Status.Count > 0)
                {
                    _logger.LogInformation("Add Status condition: {Status}", queryModel.Status.ToString());
                    filter.AddAnd((TeacherEntity entity) => entity.Status, queryModel.Status);
                }
                #endregion

                query = _filterBuilder.BuildFilterQuery(query, filter);

                var paginatedData = await query
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
                return new PaginationModel<TeacherDisplayModel>
                {
                    TotalCount = query.Count(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    DataList = from item in paginatedData
                               select item.ToModel()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while getting list of all teachers. Error: {ex}", ex);
                throw;
            }
        }

        public async ValueTask<IEnumerable<TeacherFilterModel>> GetAllTeachersFilter()
        {
            return await _context.TeacherEntities.Select(t => new TeacherFilterModel
            {
                TeacherId = t.TeacherId,
                FullName = t.FullName
            }).ToListAsync();
        }
        #endregion

        #region Get a record
        /// <summary>
        /// Get a teacher by ID (string)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async ValueTask<TeacherFullDisplayModel> GetTeacherById(string id)
        {
            try
            {
                _logger.LogInformation("Start to get a teacher by id.");
                var teacher = await _context.TeacherEntities.FindAsync(id);
                if (teacher == null)
                {
                    throw new NotFoundException($"Teacher with ID {id} not found.");
                }

                var detailModel = teacher.ToFullDetailModel();

                return detailModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while getting teacher by id. Error: {ex}", ex);
                throw;
            }
        }

        /// <summary>
        /// Get a teacher by account ID (Guid). Use for account service
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async ValueTask<TeacherFullDisplayModel> GetTeacherByAccountId(Guid id)
        {
            try
            {
                _logger.LogInformation("Start to get a teacher by account id.");
                var teacher = await _context.TeacherEntities.FirstOrDefaultAsync(s => s.AccountId == id);
                if (teacher == null)
                {
                    throw new NotFoundException($"Teacher with ID {id} not found.");
                }

                var detailModel = teacher.ToFullDetailModel();

                return detailModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while getting teacher by account id. Error: {ex}", ex);
                throw;
            }
        }
        #endregion

        #region Create
        /// <summary>
        /// Create a new teacher
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async ValueTask CreateTeacher(TeacherAddModel model)
        {
            try
            {
                _logger.LogInformation("Start to create new teacher.");
                var existingTeacher = await _context.TeacherEntities.FirstOrDefaultAsync(s => s.TeacherId == model.TeacherId);

                if (existingTeacher != null)
                {
                    _logger.LogInformation("Record has already existed");
                    throw ExistRecordException.ExistsRecord("Teacher ID already exists");
                }

                // automatically create account when creating new teacher
                AccountAutomaticallyAddModel addAccount = new()
                {
                    UserName = GenerateUniqueEmail(model.FullName),
                    Role = RoleType.Teacher
                };
                var newAccount = await _account.CreateAccountAutomatically(addAccount);

                // Upload avatar to Cloudinary if provided
                string? avatarUrl = null;
                if (model.Avatar != null)
                {
                    avatarUrl = await _cloudinary.UploadImageAsync(model.Avatar);
                }

                var newTeacher = new TeacherEntity()
                {
                    TeacherId = model.TeacherId,
                    FullName = model.FullName,
                    DOB = model.DOB,
                    IdentificationNumber = model.IdentificationNumber,
                    Gender = model.Gender,
                    Address = model.Address,
                    Ethnic = model.Ethnic,
                    PhoneNumber = model.PhoneNumber,
                    Avatar = avatarUrl,
                    Email = model.Email,
                    TimeStart = model.TimeStart,
                    TimeEnd = model.TimeEnd,
                    Level = model.Level,
                    Role = TeacherRole.Regular,
                    AccountId = newAccount.AccountId,
                    Status = TeacherStatusType.Active,
                    DepartmentId = null,
                };

                _context.TeacherEntities.Add(newTeacher);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while creating new teacher. Error: {ex}", ex);
                throw;
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Update a teacher by his/her ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async ValueTask UpdateTeacher(string id, TeacherUpdateModel model)
        {
            try
            {
                _logger.LogInformation("Start to update a teacher.");
                var teacher = await _context.TeacherEntities.FirstOrDefaultAsync(s => s.TeacherId == id);
                if (teacher == null)
                {
                    throw new NotFoundException($"Teacher with ID {id} not found.");
                }
                string? avatarUrl = null;
                if (model.Avatar != null)
                {
                    avatarUrl = await _cloudinary.UploadImageAsync(model.Avatar);
                }
                teacher.FullName = model.FullName;
                teacher.DOB = model.DOB;
                teacher.IdentificationNumber = model.IdentificationNumber;
                teacher.Gender = model.Gender;
                teacher.Address = model.Address;
                teacher.Ethnic = model.Ethnic;
                teacher.PhoneNumber = model.PhoneNumber;
                teacher.Avatar = avatarUrl;
                teacher.Email = model.Email;
                teacher.TimeEnd = model.TimeEnd;
                teacher.Level = model.Level;
                teacher.Status = model.Status;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while updating a teacher. Error: {ex}", ex);
                throw;
            }
        }    
        #endregion

        #region Delete
        /// <summary>
        /// Delete teacher by his/her ID (for testing)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async ValueTask DeleteTeacher(string id)
        {
            try
            {
                _logger.LogInformation("Start deleting teacher with ID {id}", id);

                var teacher = await _context.TeacherEntities.FirstOrDefaultAsync(s => s.TeacherId == id);

                if (teacher == null)
                {
                    throw new NotFoundException($"Teacher with ID {id} not found.");
                }

                _context.TeacherEntities.Remove(teacher);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Teacher with ID {id} deleted successfully.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while deleting Teacher with ID {id}. Error: {ex}", id, ex);
                throw;
            }
        }
        #endregion

        #region Export
        public async Task<byte[]> ExportToExcelAsync(TeacherExportQueryModel queryModel)
        {
            try
            {
                // will add search func later
                var teachers = new List<TeacherEntity>();

                // Lọc dữ liệu theo các điều kiện trong query
                if (queryModel.TeacherIds.Any())
                {
                    teachers = await _context.TeacherEntities.Where(s => queryModel.TeacherIds.Contains(s.TeacherId)).ToListAsync();
                }
                else
                {
                    teachers = await _context.TeacherEntities.ToListAsync();
                }
                // Lọc theo danh sách trạng thái nếu có
                if (queryModel.Status.Any())
                {
                    teachers = teachers.Where(s => queryModel.Status.Contains(s.Status)).ToList();
                }

                /// Dịch enum sang tiếng Việt
                var convertedStudents = teachers.Select(s => new
                {
                    s.TeacherId,
                    s.FullName,
                    s.DOB,
                    s.IdentificationNumber,
                    s.Gender,
                    s.Ethnic,
                    s.Address,
                    s.PhoneNumber,
                    //s.Email,
                    s.TimeStart,
                    s.TimeEnd,
                    s.Level,
                    Status = TranslateStatus(s.Status),
                });

                // Tạo DataTable và thêm dữ liệu sinh viên vào đó
                DataTable dt = new DataTable();
                dt.Columns.Add("MSGV", typeof(string));
                dt.Columns.Add("Họ và tên", typeof(string));
                dt.Columns.Add("Ngày sinh", typeof(string));
                dt.Columns.Add("CCCD", typeof(string));
                dt.Columns.Add("Giới tính", typeof(string));
                dt.Columns.Add("Dân tộc", typeof(string));
                dt.Columns.Add("Địa chỉ", typeof(string));
                dt.Columns.Add("SĐT", typeof(string));
                dt.Columns.Add("Trình độ", typeof(string));
                dt.Columns.Add("Bắt đầu", typeof(string));
                dt.Columns.Add("Kết thúc", typeof(string));
                dt.Columns.Add("Tình trạng giảng dạy", typeof(string));

                foreach (var st in convertedStudents)
                {
                    dt.Rows.Add(st.TeacherId,
                        st.FullName,
                        st.DOB.ToString("dd/MM/yyyy"),
                        st.IdentificationNumber,
                        st.Gender,
                        st.Ethnic,
                        st.Address,
                        st.PhoneNumber,
                        st.Level,
                        st.TimeStart.ToString("dd/MM/yyyy"),
                        st.TimeEnd.HasValue ? st.TimeEnd.Value.ToString("dd/MM/yyyy") : string.Empty,
                        st.Status);
                }

                // Xuất dữ liệu sang tệp Excel
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Danh sách Giáo viên");
                    worksheet.Cell(1, 1).InsertTable(dt);

                    // Lưu tệp Excel vào một mảng byte[]
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        workbook.SaveAs(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while exporting teachers. Error: {ex}", ex);
                throw;
            }
        }
        #endregion

        /// <summary>
        /// Translate StatusType enum
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private static string TranslateStatus(TeacherStatusType status)
        {
            switch (status)
            {
                case TeacherStatusType.Active:
                    return "Đang giảng dạy";
                case TeacherStatusType.Suspended:
                    return "Tạm nghỉ";
                case TeacherStatusType.Inactive:
                    return "Nghỉ việc";
                default:
                    return string.Empty;
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

        /// <summary>
        /// Use to check if user name is duplicated or not, if duplicated => add number behind email
        /// For example: abc1@mail.com
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        private string GenerateUniqueEmail(string fullName)
        {
            string emailBase = StringExtensions.RemoveAccents(fullName).ToLowerInvariant().Replace(" ", "");
            string email = emailBase + "@thptquangtrung.edu";

            // Kiểm tra nếu email gốc đã tồn tại
            if (_context.AccountEntities.Any(a => a.UserName == email))
            {
                int count = 1;
                // Nếu email gốc tồn tại, thì tìm số tiếp theo để thêm vào email mới
                while (_context.AccountEntities.Any(a => a.UserName == email))
                {
                    email = $"{emailBase}{count}@thptquangtrung.edu";
                    count++;
                }
            }

            return email;
        }

    }
}
