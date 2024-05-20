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

namespace SchoolManagement.Service
{
    public class TeacherService : ITeacherService
    {
        private readonly ILogger<StudentService> _logger;
        private readonly SchoolManagementDbContext _context;
        private readonly IEntityFilterService<TeacherEntity> _filterBuilder;
        private readonly ICloudinaryService _cloudinary;
        private readonly IAccountService _account;

        public TeacherService(ILogger<StudentService> logger,
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
        //public async ValueTask CreateTeacher(StudentAddModel model)
        //{
        //    try
        //    {
        //        _logger.LogInformation("Start to create new student.");
        //        // create Student ID: Current Year + ID
        //        var studentId = DateTime.Now.Year.ToString() + model.StudentId;
        //        var existingStudent = await _context.StudentEntities.FirstOrDefaultAsync(s => s.StudentId == studentId);

        //        if (existingStudent != null)
        //        {
        //            _logger.LogInformation("Record has already existed");
        //            throw ExistRecordException.ExistsRecord("Student ID already exists");
        //        }

        //        // automatically create account when creating new student
        //        AccountAutomaticallyAddModel addAccount = new()
        //        {
        //            UserName = StringExtensions.GenerateEmailBasedFullName(model.FullName, studentId),
        //            Role = RoleType.Student
        //        };
        //        var newAccount = await _account.CreateAccountAutomatically(addAccount);

        //        // Upload avatar to Cloudinary if provided
        //        string? avatarUrl = null;
        //        if (model.Avatar != null)
        //        {
        //            avatarUrl = await _cloudinary.UploadImageAsync(model.Avatar);
        //        }

        //        var newStudent = new StudentEntity()
        //        {
        //            StudentId = studentId,
        //            FullName = model.FullName,
        //            DOB = model.DOB,
        //            IdentificationNumber = model.IdentificationNumber,
        //            Gender = model.Gender,
        //            Address = model.Address,
        //            Ethnic = model.Ethnic,
        //            PhoneNumber = model.PhoneNumber,
        //            Avatar = avatarUrl,
        //            Email = model.Email,
        //            Status = StudentStatusType.Active,
        //            FatherName = model.FatherName,
        //            FatherJob = model.FatherJob,
        //            FatherPhoneNumber = model.FatherPhoneNumber,
        //            FatherEmail = model.FatherEmail,
        //            MotherName = model.MotherName,
        //            MotherJob = model.MotherJob,
        //            MotherPhoneNumber = model.MotherPhoneNumber,
        //            MotherEmail = model.MotherEmail,
        //            AcademicYear = DateTime.Now.Year.ToString() + " - " + DateTime.Now.AddYears(3).Year.ToString(),
        //            AccountId = newAccount.AccountId
        //        };

        //        _context.StudentEntities.Add(newStudent);
        //        await _context.SaveChangesAsync();

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("An error occured while creating new student. Error: {ex}", ex);
        //        throw;
        //    }
        //}
        #endregion

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
