using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Common.Enum;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Database;
using SchoolManagement.Entity;
using SchoolManagement.Model;
using SchoolManagement.Service.Intention;
using SchoolManagement.Service.Intention.Data;

namespace SchoolManagement.Service
{
    public class ClassService : IClassService
    {
        private readonly ILogger<ClassService> _logger;
        private readonly SchoolManagementDbContext _context;
        private readonly IEntityFilterService<ClassEntity> _filterBuilder;
        private readonly ITeacherService _teacherService;

        public ClassService(ILogger<ClassService> logger, 
            SchoolManagementDbContext context, 
            IEntityFilterService<ClassEntity> filterBuilder,
            ITeacherService teacherService)
        {
            _logger = logger;
            _context = context;
            _filterBuilder = filterBuilder;
            _teacherService = teacherService;
        }

        /// <summary>
        /// Get list of classes by grade
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public async ValueTask<PaginationModel<ClassDisplayModel>> GetAllClasses(int grade, PageQueryModel queryModel)
        {
            try
            {
                _logger.LogInformation("Start to get list classes of grade {Grade}.", grade);

                int pageNumber = queryModel.PageNumber != null && queryModel.PageNumber.Value > 0 ? queryModel.PageNumber.Value : 1;
                int pageSize = queryModel.PageSize != null && queryModel.PageSize.Value > 0 ? queryModel.PageSize.Value : 10;

                // Kiểm tra nếu không tìm thấy lớp học nào với grade được cung cấp
                var existsGrade = await _context.ClassEntities.AnyAsync(c => c.Grade == grade);
                if (!existsGrade)
                {
                    _logger.LogWarning("No classes found for grade {Grade}.", grade);
                    return new PaginationModel<ClassDisplayModel>
                    {
                        TotalCount = 0,
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        DataList = new List<ClassDisplayModel>()
                    };
                }

                var query = _context.ClassEntities
                    .Where(c => c.Grade == grade)
                    .Include(c => c.HomeroomTeacher) // Include the related teacher entity
                    .AsQueryable();

                #region Search
                if (!string.IsNullOrEmpty(queryModel.SearchValue))
                {
                    _logger.LogInformation("Add Search Value: {SearchValue}", queryModel.SearchValue);
                    query = query.Where(c => c.ClassId.Contains(queryModel.SearchValue) ||
                                             c.ClassName.Contains(queryModel.SearchValue) ||
                                             c.AcademicYear.Contains(queryModel.SearchValue) ||
                                             c.HomeroomTeacher.FullName.ToLower().Contains(queryModel.SearchValue.ToLower()));
                }
                #endregion

                var totalCount = await query.CountAsync();

                var paginatedData = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(c => new ClassDisplayModel
                    {
                        ClassId = c.ClassId,
                        ClassName = c.ClassName,
                        AcademicYear = c.AcademicYear,
                        Grade = c.Grade,
                        HomeroomTeacherId = c.HomeroomTeacherId,
                        HomeroomTeacherName = c.HomeroomTeacher.FullName // Map the teacher's name
                    })
                    .OrderByDescending(c => c.AcademicYear)
                    .ThenBy(c => c.ClassName)
                    .ToListAsync();

                return new PaginationModel<ClassDisplayModel>
                {
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    DataList = paginatedData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting list of all classes in grade {Grade}. Error: {Error}", grade, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Get classes belong to HR teacher
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async ValueTask<PaginationModel<HomeroomClassDisplayModel>> GetAllClassesByAccountId(Guid accountId, PageQueryModel queryModel)
        {
            try
            {
                _logger.LogInformation("Start to get list of classes for account ID {AccountId}.", accountId);

                int pageNumber = queryModel.PageNumber != null && queryModel.PageNumber.Value > 0 ? queryModel.PageNumber.Value : 1;
                int pageSize = queryModel.PageSize != null && queryModel.PageSize.Value > 0 ? queryModel.PageSize.Value : 10;

                // Tìm giáo viên có accountId
                var teacher = await _context.TeacherEntities
                    .Include(t => t.Department)
                    .FirstOrDefaultAsync(t => t.AccountId == accountId);

                if (teacher == null)
                {
                    _logger.LogWarning("Teacher with account ID {AccountId} not found.", accountId);
                    throw new NotFoundException("Teacher not found.");
                }

                // Truy vấn các lớp mà giáo viên này làm chủ nhiệm
                var query = _context.ClassEntities
                    .Where(c => c.HomeroomTeacherId == teacher.TeacherId)
                    .Select(c => new HomeroomClassDisplayModel
                    {
                        ClassId = c.ClassId,
                        ClassName = c.ClassName,
                        AcademicYear = c.AcademicYear,
                        Grade = c.Grade,
                        TotalStudents = _context.ClassDetailEntities.Count(s => s.ClassId == c.ClassId)
                    });

                // Lấy tổng số bản ghi
                var totalCount = await query.CountAsync();

                // Lấy dữ liệu theo phân trang
                var classList = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // Tạo đối tượng phân trang để trả về
                var result = new PaginationModel<HomeroomClassDisplayModel>
                {
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    DataList = classList
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting list of classes for account ID {AccountId}. Error: {Error}", accountId, ex.Message);
                throw;
            }
        }

        public async ValueTask<PaginationModel<NormalClassDisplayModel>> GetClassesAssignedForTeacher(Guid accountId, string semesterId, PageModel queryModel)
        {
            try
            {
                _logger.LogInformation("Fetching classes assigned for teacher with account ID {AccountId} for semester {SemesterId}.", accountId, semesterId);

                // Tìm kiếm giáo viên với accountId
                var teacher = await _context.TeacherEntities
                    .Include(t => t.Account)
                    .FirstOrDefaultAsync(t => t.AccountId == accountId);

                if (teacher == null)
                {
                    _logger.LogWarning("No teacher found with account ID {AccountId}.", accountId);
                    throw new NotFoundException("Teacher not found.");
                }

                // Lấy danh sách các lớp mà giáo viên giảng dạy trong học kỳ cụ thể
                var assignedClassesQuery = _context.AssignmentEntities
                    .Where(a => a.TeacherId == teacher.TeacherId && a.SemesterId == semesterId)
                    .Select(a => new NormalClassDisplayModel
                    {
                        ClassId = a.Class.ClassId,
                        ClassName = a.Class.ClassName,
                        AcademicYear = a.Class.AcademicYear,
                        Grade = a.Class.Grade,
                        SubjectId = a.SubjectId,
                        SubjectName = a.Subject.SubjectName,
                        TotalStudents = a.Class.ClassDetails.Count
                    })
                    .Distinct(); // Loại bỏ các lớp trùng lặp trong trường hợp giáo viên dạy nhiều môn cùng lớp;

                int pageNumber = queryModel.PageNumber != null && queryModel.PageNumber.Value > 0 ? queryModel.PageNumber.Value : 1;
                int pageSize = queryModel.PageSize != null && queryModel.PageSize.Value > 0 ? queryModel.PageSize.Value : 10;

                var totalCount = await assignedClassesQuery.CountAsync();
                var assignedClasses = await assignedClassesQuery
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                _logger.LogInformation("Successfully retrieved {ClassCount} classes assigned for teacher with account ID {AccountId} for semester {SemesterId}.", assignedClasses.Count, accountId, semesterId);

                return new PaginationModel<NormalClassDisplayModel>
                {
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    DataList = assignedClasses
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while fetching classes assigned for teacher with account ID {AccountId} for semester {SemesterId}. Error: {Error}", accountId, semesterId, ex.Message);
                throw;
            }
        }



        /// <summary>
        /// Get list classes in 1 semester. This dto is for 1st conduct and assessment display
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="semesterId"></param>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public async ValueTask<IEnumerable<ClassInSemesterModel>> GetListClassesInSemester(int grade, string semesterId)
        {
            try
            {
                // Kiểm tra xem semesterId có tồn tại trong cơ sở dữ liệu không
                var semesterExists = await _context.SemesterEntities.AnyAsync(s => s.SemesterId == semesterId);

                // Nếu semesterId không tồn tại, trả về danh sách rỗng
                if (!semesterExists)
                {
                    return Enumerable.Empty<ClassInSemesterModel>();

                }

                var classesInSemester = await _context.ClassEntities
                    .Where(c => c.Grade == grade)
                    .Where(c => _context.SemesterDetailEntities
                        .Any(sd => sd.ClassId == c.ClassId && sd.SemesterId == semesterId))
                    .Select(c => new ClassInSemesterModel
                    {
                        ClassId = c.ClassId,
                        ClassName = c.ClassName,
                        AcademicYear = c.AcademicYear,
                        TotalStudents = _context.ClassDetailEntities.Count(cd => cd.ClassId == c.ClassId),
                        HomeroomTeacherName = c.HomeroomTeacher.FullName
                    })
                    .ToListAsync();

                // Thêm logic filter ở đây nếu cần

                return classesInSemester;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while fetching classes in semester. Error: {Error}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Get list classes in 1 semester. This dto is for 1st conduct and assessment display
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="semesterId"></param>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public async ValueTask<IEnumerable<ClassInAcademicYearModel>> GetListClassesInAcademicYear(int grade, string academicYear)
        {
            try
            {
                // Kiểm tra xem academic year có tồn tại trong cơ sở dữ liệu không
                var acaYearExists = await _context.SemesterEntities.AnyAsync(s => s.AcademicYear == academicYear);

                // Nếu semesterId không tồn tại, trả về danh sách rỗng
                if (!acaYearExists)
                {
                    return Enumerable.Empty<ClassInAcademicYearModel>();

                }

                var classesInAcaYear = await _context.ClassEntities
                    .Where(c => c.Grade == grade && c.AcademicYear == academicYear)
                    .Select(c => new ClassInAcademicYearModel
                    {
                        ClassId = c.ClassId,
                        ClassName = c.ClassName,
                        AcademicYear = c.AcademicYear,
                        HomeroomTeacherName = c.HomeroomTeacher.FullName
                    })
                    .ToListAsync();

                // Thêm logic filter ở đây nếu cần

                return classesInAcaYear;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while fetching classes in semester. Error: {Error}", ex.Message);
                throw;
            }
        }

        /// <summary>
        ///  filter: get HR teacher (bug: need to add academicYear to sort out teachers who already chủ nhiệm in 1 niên khóa (done)
        /// </summary>
        /// <param name="grade"></param>
        /// <returns></returns>
        public async ValueTask<IEnumerable<TeacherFilterModel>> GetAvailableTeachersByGradeAsync(string academicYear)
        {
            try
            {
                _logger.LogInformation($"Starting to filter available teachers for academic year {academicYear}.");

                // Lấy danh sách giáo viên chưa chủ nhiệm lớp nào trong khối chỉ định và niên khóa chỉ định
                var availableTeachers = await _context.TeacherEntities
                    .Where(t => !t.Classes.Any(c => c.AcademicYear == academicYear))
                    .Select(t => new TeacherFilterModel
                    {
                        TeacherId = t.TeacherId,
                        FullName = t.FullName
                    })
                    .ToListAsync();

                _logger.LogInformation($"Found {availableTeachers.Count} available teachers for academic year {academicYear}.");

                return availableTeachers;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while filtering teachers. Error: {ex}", ex);
                throw;
            }
        }


        public async ValueTask<IEnumerable<ClassFilterModel>> GetClassesByGradeFilter(int grade, string academicYear)
        {
            try
            {
                var classes = await _context.ClassEntities
                    .Where(c => c.Grade == grade && c.AcademicYear == academicYear)
                    .Select(c => new ClassFilterModel
                    {
                        ClassId = c.ClassId,
                        ClassName = c.ClassName
                    })
                    .ToListAsync();

                return classes;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting classes by grade. Error: {ex}", ex);
                throw;
            }
        }

        /// <summary>
        /// Not used for api
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async ValueTask<string> GetClassNameById(string classId)
        {
            var classInfo = await _context.ClassEntities
                    .Where(c => c.ClassId == classId)
                    .Select(c => new { c.ClassName })
                    .FirstOrDefaultAsync();
            if (classInfo == null)
            {
                throw new ArgumentException($"Class with ID {classId} not found.");
            }
            var className = classInfo.ClassName;
            return className;
        }

        #region Create
        /// <summary>
        /// Add new class in 1 grade (will change class id format -done)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async ValueTask CreateClass(ClassAddModel model)
        {
            try
            {
                _logger.LogInformation("Starting to create new class in grade {Grade} for academic year {AcademicYear}.", model.Grade, model.AcademicYear);

                // Generate the prefix for the ClassId
                var prefixClassId = GenerateClassId(model.ClassName, model.AcademicYear);
                var existClass = await _context.ClassEntities
                    .FirstOrDefaultAsync(s => s.ClassId == prefixClassId && s.Grade == model.Grade);

                if (existClass != null)
                {
                    _logger.LogWarning("Class ID {ClassId} already exists for grade {Grade}.", prefixClassId, model.Grade);
                    throw ExistRecordException.ExistsRecord("Class ID already exists");
                }

                // Check if HomeroomTeacherId is already assigned to another class
                var isHomeroomTeacherInUse = await _context.ClassEntities
                    .AnyAsync(c => c.HomeroomTeacherId == model.HomeroomTeacherId && c.Grade == model.Grade && c.AcademicYear == model.AcademicYear);

                if (isHomeroomTeacherInUse)
                {
                    _logger.LogWarning("Homeroom teacher with ID {HomeroomTeacherId} is already assigned to another class in grade {Grade} for academic year {AcademicYear}.",
                                        model.HomeroomTeacherId, model.Grade, model.AcademicYear);
                    throw new InvalidOperationException($"Homeroom teacher with ID {model.HomeroomTeacherId} is already assigned to another class");
                }

                // Begin transaction
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Update teacher's role to HomeroomTeacher
                        await _teacherService.UpdateTeacherRoll(model.HomeroomTeacherId, RoleType.HomeroomTeacher);
                        _logger.LogInformation("Updated role to HomeroomTeacher for teacher ID {HomeroomTeacherId}.", model.HomeroomTeacherId);

                        // Create new class entity
                        var newClass = new ClassEntity()
                        {
                            ClassId = prefixClassId,
                            ClassName = model.ClassName,
                            AcademicYear = model.AcademicYear,
                            Grade = model.Grade,
                            CreatedAt = DateTime.UtcNow,
                            ModifiedAt = null,
                            HomeroomTeacherId = model.HomeroomTeacherId,
                        };

                        _context.ClassEntities.Add(newClass);
                        await _context.SaveChangesAsync();
                        _logger.LogInformation("Successfully created class ID {ClassId} for grade {Grade} in academic year {AcademicYear}.", prefixClassId, model.Grade, model.AcademicYear);

                        // Commit transaction
                        await transaction.CommitAsync();
                        _logger.LogInformation("Transaction committed successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction if any error occurs
                        await transaction.RollbackAsync();
                        _logger.LogError("An error occurred while creating new class. Transaction rolled back. Error: {Error}", ex.Message);
                        throw;
                    }
                }
            }
            catch (ExistRecordException ex)
            {
                _logger.LogWarning("Failed to create class. Error: {Error}", ex.Message);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Invalid operation while creating class. Error: {Error}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occurred while creating new class. Error: {Error}", ex.Message);
                throw;
            }
        }

        #endregion

        #region Update
        /// <summary>
        /// Update a class by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async ValueTask UpdateClass(string classId, string academicYear, ClassUpdateModel model)
        {
            try
            {
                _logger.LogInformation("Start to update class with ID {ClassId}.", classId);

                var existingClass = await _context.ClassEntities.FirstOrDefaultAsync(c => c.ClassId == classId);
                if (existingClass == null)
                {
                    _logger.LogWarning("Class with ID {ClassId} not found.", classId);
                    throw new NotFoundException("Class not found.");
                }

                // Check if the HomeroomTeacherId is changing
                var isHomeroomTeacherChanging = !string.IsNullOrEmpty(model.HoomroomTeacherId) &&
                                                existingClass.HomeroomTeacherId != model.HoomroomTeacherId;

                // Begin transaction to ensure data integrity
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        if (isHomeroomTeacherChanging)
                        {
                            // Check if the new HomeroomTeacherId is already assigned to another class
                            var teacherAlreadyAssigned = await _context.ClassEntities
                                .AnyAsync(c => c.HomeroomTeacherId == model.HoomroomTeacherId && c.ClassId != classId && c.AcademicYear == academicYear);
                            if (teacherAlreadyAssigned)
                            {
                                _logger.LogWarning("Homeroom teacher with ID {HomeroomTeacherId} is already assigned to another class.", model.HoomroomTeacherId);
                                throw new InvalidOperationException("Homeroom teacher is already assigned to another class.");
                            }

                            // Update the old HomeroomTeacher's role back to Teacher
                            if (!string.IsNullOrEmpty(existingClass.HomeroomTeacherId))
                            {
                                await _teacherService.UpdateTeacherRoll(existingClass.HomeroomTeacherId, RoleType.Teacher);
                                _logger.LogInformation("Updated role to Teacher for previous homeroom teacher ID {OldHomeroomTeacherId}.", existingClass.HomeroomTeacherId);
                            }

                            // Update the new HomeroomTeacher's role to HomeroomTeacher
                            await _teacherService.UpdateTeacherRoll(model.HoomroomTeacherId, RoleType.HomeroomTeacher);
                            _logger.LogInformation("Updated role to HomeroomTeacher for new homeroom teacher ID {NewHomeroomTeacherId}.", model.HoomroomTeacherId);
                        }

                        // Update class information
                        existingClass.ClassName = model.ClassName;
                        existingClass.HomeroomTeacherId = model.HoomroomTeacherId;
                        existingClass.ModifiedAt = DateTime.UtcNow;

                        _context.ClassEntities.Update(existingClass);
                        await _context.SaveChangesAsync();

                        // Commit transaction
                        await transaction.CommitAsync();
                        _logger.LogInformation("Class with ID {ClassId} updated successfully.", classId);
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction if any error occurs
                        await transaction.RollbackAsync();
                        _logger.LogError("An error occurred while updating class with ID {ClassId}. Transaction rolled back. Error: {Error}", classId, ex.Message);
                        throw;
                    }
                }
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Failed to update class. Class not found. Error: {Error}", ex.Message);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Invalid operation while updating class. Error: {Error}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occurred while updating class with ID {ClassId}. Error: {Error}", classId, ex.Message);
                throw;
            }
        }

        #endregion

        /// <summary>
        /// Delete a class by id
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async ValueTask DeleteClass(string classId)
        {
            try
            {
                _logger.LogInformation("Start to delete class with ID {ClassId}.", classId);

                var existingClass = await _context.ClassEntities.FirstOrDefaultAsync(c => c.ClassId == classId);
                if (existingClass == null)
                {
                    _logger.LogWarning("Class with ID {ClassId} not found.", classId);
                    throw new NotFoundException("Class not found.");
                }

                _context.ClassEntities.Remove(existingClass);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Class with ID {ClassId} deleted successfully.", classId);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while deleting class with ID {ClassId}. Error: {Error}", classId, ex.Message);
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

        //private string GenerateClassId(string className, string semester, string academicYear)
        //{
        //    // Tách năm học thành hai phần: năm bắt đầu và năm kết thúc
        //    var years = academicYear.Split(" - ");
        //    if (years.Length != 2)
        //    {
        //        throw new ArgumentException("Invalid academic year format. Expected format: 'YYYY - YYYY'.");
        //    }

        //    var startYear = years[0];
        //    var endYear = years[1];

        //    // Xác định năm dựa trên học kỳ
        //    var year = semester == "1" ? startYear : semester == "2" ? endYear : throw new ArgumentException("Invalid semester. Expected '1' or '2'.");

        //    // Tạo chuỗi kết quả
        //    var result = $"{year}{className}";
        //    return result;
        //}
        private string GenerateClassId(string className, string academicYear)
        {
            // Kiểm tra tham số đầu vào
            if (string.IsNullOrWhiteSpace(className))
            {
                throw new ArgumentException("Class name cannot be empty or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(academicYear))
            {
                throw new ArgumentException("Academic year cannot be empty or whitespace.");
            }

            // Chuẩn hóa academicYear để hỗ trợ cả hai định dạng 'YYYY - YYYY' và 'YYYY-YYYY'
            var normalizedAcademicYear = academicYear.Replace(" ", "").Trim();
            var years = normalizedAcademicYear.Split('-');

            if (years.Length != 2)
            {
                throw new ArgumentException("Invalid academic year format. Expected format: 'YYYY - YYYY' or 'YYYY-YYYY'.");
            }

            var startYear = years[0].Trim();
            var endYear = years[1].Trim();

            // Kiểm tra độ dài của các năm
            if (startYear.Length != 4 || endYear.Length != 4)
            {
                throw new ArgumentException("Invalid year length. Expected format: 'YYYY'.");
            }

            // Kiểm tra nếu các giá trị năm là số nguyên và năm kết thúc lớn hơn năm bắt đầu
            if (!int.TryParse(startYear, out int startYearInt) || !int.TryParse(endYear, out int endYearInt))
            {
                throw new ArgumentException("Invalid year format. Years must be numeric.");
            }

            if (endYearInt <= startYearInt)
            {
                throw new ArgumentException("End year must be greater than start year.");
            }

            // Chuẩn hóa tên lớp: Loại bỏ khoảng trắng thừa
            var normalizedClassName = className.Trim();

            // Tạo chuỗi kết quả
            var result = $"{startYear}{endYear}{normalizedClassName}";

            return result;
        }


    }
}
