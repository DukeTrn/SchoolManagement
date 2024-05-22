using ClosedXML.Excel;
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
    public class StudentService : IStudentService
    {
        private readonly ILogger<StudentService> _logger;
        private readonly SchoolManagementDbContext _context;
        private readonly IEntityFilterService<StudentEntity> _filterBuilder;
        private readonly ICloudinaryService _cloudinary;
        private readonly IAccountService _account;

        public StudentService(ILogger<StudentService> logger,
            IEntityFilterService<StudentEntity> filterBuilder,
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


        #region List students
        /// <summary>
        /// Get list of all students with pagination
        /// will add status filter (if any)
        /// </summary>
        /// <param name="queryModel"></param>
        public async ValueTask<PaginationModel<StudentDisplayModel>> GetAllStudents(StudentQueryModel queryModel)
        {
            try
            {
                _logger.LogInformation("Start to get list students.");
                int pageNumber = queryModel.PageNumber != null && queryModel.PageNumber.Value > 0 ? queryModel.PageNumber.Value : 1;
                int pageSize = queryModel.PageSize != null && queryModel.PageSize.Value > 0 ? queryModel.PageSize.Value : 10;

                FilterModel filter = new();
                var query = _context.StudentEntities.AsQueryable();

                #region Search
                if (!string.IsNullOrEmpty(queryModel.SearchValue))
                {
                    _logger.LogInformation("Add Search Value: {SearchValue}", queryModel.SearchValue);
                    var searchFilter = BuildSearchFilter(queryModel.SearchValue,
                        nameof(StudentEntity.StudentId),
                        nameof(StudentEntity.FullName),
                        nameof(StudentEntity.Gender),
                        nameof(StudentEntity.Email),
                        nameof(StudentEntity.PhoneNumber));
                    filter.Or.AddRange(searchFilter);
                }
                #endregion

                #region Status Filter
                if (queryModel.Status.Count > 0)
                {
                    _logger.LogInformation("Add Status condition: {Status}", queryModel.Status.ToString());
                    filter.AddAnd((StudentEntity entity) => entity.Status, queryModel.Status);
                }
                #endregion

                query = _filterBuilder.BuildFilterQuery(query, filter);

                var paginatedData = await query
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
                return new PaginationModel<StudentDisplayModel>
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
                _logger.LogError("An error occured while getting list of all students. Error: {ex}", ex);
                throw;
            }
        }
        #endregion

        #region Get a record
        /// <summary>
        /// Get a student by ID (string)
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public async ValueTask<StudentFullDetailModel> GetStudentById(string studentId)
        {
            try
            {
                _logger.LogInformation("Start to get a student by id.");
                var student = await _context.StudentEntities.FindAsync(studentId);
                if (student == null)
                {
                    throw new NotFoundException($"Student with ID {studentId} not found.");
                }

                // Chuyển đổi StudentEntity thành StudentFullDetailModel (nếu cần)
                // Map properties from student entity to display model
                var detailModel = student.ToFullDetailModel();

                return detailModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while getting student by id. Error: {ex}", ex);
                throw;
            }
        }

        /// <summary>
        /// Get a student by account ID (Guid). Use for account service
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async ValueTask<StudentFullDetailModel> GetStudentByAccountId(Guid accountId)
        {
            try
            {
                _logger.LogInformation("Start to get a student by account id.");
                var student = await _context.StudentEntities.FirstOrDefaultAsync(s => s.AccountId == accountId);
                if (student == null)
                {
                    throw new NotFoundException($"Student with ID {accountId} not found.");
                }

                // Chuyển đổi StudentEntity thành StudentFullDetailModel (nếu cần)
                // Map properties from student entity to display model
                var detailModel = student.ToFullDetailModel();

                return detailModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while getting student by account id. Error: {ex}", ex);
                throw;
            }
        }
        #endregion

        #region Create
        /// <summary>
        /// Add new student
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async ValueTask CreateStudent(StudentAddModel model)
        {
            try
            {
                _logger.LogInformation("Start to create new student.");
                // create Student ID: Current Year + ID
                var studentId = DateTime.Now.Year.ToString() + model.StudentId;
                var existingStudent = await _context.StudentEntities.FirstOrDefaultAsync(s => s.StudentId == studentId);

                if (existingStudent != null)
                {
                    _logger.LogInformation("Record has already existed");
                    throw ExistRecordException.ExistsRecord("Student ID already exists");
                }

                // automatically create account when creating new student
                AccountAutomaticallyAddModel addAccount = new()
                {
                    UserName = StringExtensions.GenerateStudentEmail(model.FullName, studentId),
                    Role = RoleType.Student
                };
                var newAccount = await _account.CreateAccountAutomatically(addAccount);

                // Upload avatar to Cloudinary if provided
                string? avatarUrl = null;
                if (model.Avatar != null)
                {
                    avatarUrl = await _cloudinary.UploadImageAsync(model.Avatar);
                }

                var newStudent = new StudentEntity()
                {
                    StudentId = studentId,
                    FullName = model.FullName,
                    DOB = model.DOB,
                    IdentificationNumber = model.IdentificationNumber,
                    Gender = model.Gender,
                    Address = model.Address,
                    Ethnic = model.Ethnic,
                    PhoneNumber = model.PhoneNumber,
                    Avatar = avatarUrl,
                    Email = model.Email,
                    Status = StudentStatusType.Active,
                    FatherName = model.FatherName,
                    FatherJob = model.FatherJob,
                    FatherPhoneNumber = model.FatherPhoneNumber,
                    FatherEmail = model.FatherEmail,
                    MotherName = model.MotherName,
                    MotherJob = model.MotherJob,
                    MotherPhoneNumber = model.MotherPhoneNumber,
                    MotherEmail = model.MotherEmail,
                    AcademicYear = DateTime.Now.Year.ToString() + " - " + DateTime.Now.AddYears(3).Year.ToString(),
                    AccountId = newAccount.AccountId
                };

                _context.StudentEntities.Add(newStudent);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while creating new student. Error: {ex}", ex);
                throw;
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Update a student by his/her ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async ValueTask UpdateStudent(string id, StudentUpdateModel model)
        {
            try
            {
                _logger.LogInformation("Start to update a student.");
                var student = await _context.StudentEntities.FirstOrDefaultAsync(s => s.StudentId == id);
                if (student == null)
                {
                    throw new NotFoundException($"Student with ID {id} not found.");
                }
                string? avatarUrl = null;
                if (model.Avatar != null)
                {
                    avatarUrl = await _cloudinary.UploadImageAsync(model.Avatar);
                }
                student.FullName = model.FullName;
                student.DOB = model.DOB;
                student.IdentificationNumber = model.IdentificationNumber;
                student.Gender = model.Gender;
                student.Address = model.Address;
                student.Ethnic = model.Ethnic;
                student.PhoneNumber = model.PhoneNumber;
                student.Avatar = avatarUrl;
                student.Email = model.Email ?? "";
                student.Status = model.Status;
                // Parent info
                student.FatherName = model.FatherName;
                student.FatherJob = model.FatherJob;
                student.FatherPhoneNumber = model.FatherPhoneNumber;
                student.FatherEmail = model.FatherEmail;
                student.MotherName = model.MotherName;
                student.MotherJob = model.MotherJob;
                student.MotherPhoneNumber = model.MotherPhoneNumber;
                student.MotherEmail = model.MotherEmail;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while updating a student. Error: {ex}", ex);
                throw;
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete student by his/her ID (for testing)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async ValueTask DeleteStudent(string id)
        {
            try
            {
                _logger.LogInformation("Start deleting student with ID {id}", id);

                var student = await _context.StudentEntities.FirstOrDefaultAsync(s => s.StudentId == id);

                if (student == null)
                {
                    throw new NotFoundException($"Student with ID {id} not found.");
                }

                _context.StudentEntities.Remove(student);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Student with ID {id} deleted successfully.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while deleting student with ID {id}. Error: {ex}", id, ex);
                throw;
            }
        }
        #endregion

        #region Export
        public async Task<byte[]> ExportToExcelAsync(StudentExportQueryModel queryModel)
        {
            try
            {
                // will add search func later
                var students = new List<StudentEntity>();

                // Lọc dữ liệu theo các điều kiện trong query
                if (queryModel.StudentIds.Any())
                {
                    students = await _context.StudentEntities.Where(s => queryModel.StudentIds.Contains(s.StudentId)).ToListAsync();
                }
                else
                {
                    students = await _context.StudentEntities.ToListAsync();
                }
                // Lọc theo danh sách trạng thái nếu có
                if (queryModel.Status.Any())
                {
                    students = students.Where(s => queryModel.Status.Contains(s.Status)).ToList();
                }

                /// Dịch enum sang tiếng Việt
                var convertedStudents = students.Select(s => new
                {
                    s.StudentId,
                    s.FullName,
                    s.DOB,
                    s.IdentificationNumber,
                    s.Gender,
                    s.Ethnic,
                    s.Address,
                    s.PhoneNumber,
                    //s.Email,
                    Status = TranslateStatus(s.Status),
                    s.FatherName,
                    s.FatherPhoneNumber,
                    s.MotherName,
                    s.MotherPhoneNumber,
                    s.AcademicYear
                });

                // Tạo DataTable và thêm dữ liệu sinh viên vào đó
                DataTable dt = new DataTable();
                dt.Columns.Add("MSHS", typeof(string));
                dt.Columns.Add("Họ và tên", typeof(string));
                dt.Columns.Add("Ngày sinh", typeof(string));
                dt.Columns.Add("CCCD", typeof(string));
                dt.Columns.Add("Giới tính", typeof(string));
                dt.Columns.Add("Dân tộc", typeof(string));
                dt.Columns.Add("Địa chỉ", typeof(string));
                dt.Columns.Add("SĐT", typeof(string));
                dt.Columns.Add("Tình trạng học tập", typeof(string));
                dt.Columns.Add("Họ và tên bố", typeof(string));
                dt.Columns.Add("SĐT bố", typeof(string));
                dt.Columns.Add("Họ và tên mẹ", typeof(string));
                dt.Columns.Add("SĐT mẹ", typeof(string));
                dt.Columns.Add("Niên khóa", typeof(string));

                foreach (var st in convertedStudents)
                {
                    dt.Rows.Add(st.StudentId,
                        st.FullName,
                        st.DOB.ToString("dd/MM/yyyy"),
                        st.IdentificationNumber,
                        st.Gender,
                        st.Ethnic,
                        st.Address,
                        st.PhoneNumber,
                        st.Status,
                        st.FatherName,
                        st.FatherPhoneNumber,
                        st.MotherName,
                        st.MotherPhoneNumber,
                        st.AcademicYear);
                }

                // Xuất dữ liệu sang tệp Excel
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Danh sách Học sinh");
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
                _logger.LogError("An error occurred while exporting students. Error: {ex}", ex);
                throw;
            }
        }
        #endregion

        // Check data (related to class detail entity)
        
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
        /// Translate StatusType enum
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private static string TranslateStatus(StudentStatusType status)
        {
            switch (status)
            {
                case StudentStatusType.Active:
                    return "Đang học";
                case StudentStatusType.Suspended:
                    return "Đình chỉ";
                case StudentStatusType.Inactive:
                    return "Nghỉ học";
                default:
                    return string.Empty;
            }
        }
    }
}
