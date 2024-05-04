using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Common.Enum;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Database;
using SchoolManagement.Entity;
using SchoolManagement.Model;
using SchoolManagement.Service.Intention;
using SchoolManagement.Service.Intention.Data;
using SchoolManagement.Share;

namespace SchoolManagement.Service
{
    public class StudentService : IStudentService
    {
        private readonly ILogger<StudentService> _logger;
        private readonly SchoolManagementDbContext _context;
        private readonly IEntityFilterService<StudentEntity> _filterBuilder;

        public StudentService(ILogger<StudentService> logger, 
            IEntityFilterService<StudentEntity> filterBuilder,
            SchoolManagementDbContext context)
        {
            _logger = logger;
            _filterBuilder = filterBuilder;
            _context = context;
        }

        /// <summary>
        /// Get list of all students
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
                //var query = _context.StudentEntities.FromSqlRaw("SELECT * FROM dbo.Students").ToList();
                var query = _context.StudentEntities.AsQueryable();
                int totalCount = await query.CountAsync();

                #region Search
                if (!string.IsNullOrEmpty(queryModel.SearchValue))
                {
                    _logger.LogInformation("Add Search Value: {SearchValue}", queryModel.SearchValue);
                    var searchFilter = BuildSearchFilter(queryModel.SearchValue,
                        nameof(StudentEntity.StudentId),
                        nameof(StudentEntity.FullName),
                        nameof(StudentEntity.IdentificationNumber),
                        nameof(StudentEntity.Gender),
                        nameof(StudentEntity.Ethnic),
                        nameof(StudentEntity.Address),
                        nameof(StudentEntity.PhoneNumber));
                    filter.Or.AddRange(searchFilter);
                }
                #endregion

                query = _filterBuilder.BuildFilterQuery(query, filter);

                var paginatedData = await query
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
                return new PaginationModel<StudentDisplayModel>
                {
                    TotalCount = totalCount,
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
                var studentId = DateTime.Now.Year.ToString() + model.StudentId;
                var existingStudent = await _context.StudentEntities.FirstOrDefaultAsync(s => s.StudentId == studentId);

                if (existingStudent != null)
                {
                    _logger.LogInformation("Record has already existed");
                    throw ExistRecordException.ExistsRecord("Student ID already exists");
                    //throw new Exception("Student ID already exists.");
                }

                else
                {
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
                        Avatar = model.Avatar,
                        Email = model.Email,
                        Status = StatusType.Active,
                        FatherName = model.FatherName,
                        FatherJob = model.FatherJob,
                        FatherPhoneNumber = model.FatherPhoneNumber,
                        FatherEmail = model.FatherEmail,
                        MotherName = model.MotherName,
                        MotherJob = model.MotherJob,
                        MotherPhoneNumber = model.MotherPhoneNumber,
                        MotherEmail = model.MotherEmail,
                        AcademicYear = DateTime.Now.Year.ToString() + " - " + DateTime.Now.AddYears(3).Year.ToString(),
                    };

                    _context.StudentEntities.Add(newStudent);
                    await _context.SaveChangesAsync();

                    //return newStudent;
                    //return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while creating new student. Error: {ex}", ex);
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
    }
}
