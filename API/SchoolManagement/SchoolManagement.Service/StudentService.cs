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
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while creating new student. Error: {ex}", ex);
                throw;
            }
        }

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
                _logger.LogInformation("Start to update new student.");
                var student = await _context.StudentEntities.FirstOrDefaultAsync(s => s.StudentId == id);
                if (student == null)
                {
                    throw new NotFoundException($"Student with ID {id} not found.");
                }
                student.FullName = model.FullName;
                student.DOB = model.DOB;
                student.IdentificationNumber = model.IdentificationNumber;
                student.Gender = model.Gender;
                student.Address = model.Address;
                student.Ethnic = model.Ethnic;
                student.PhoneNumber = model.PhoneNumber;
                student.Avatar = model.Avatar;
                student.Email = model.Email;
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

        /// <summary>
        /// Delete student by his/her ID
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
