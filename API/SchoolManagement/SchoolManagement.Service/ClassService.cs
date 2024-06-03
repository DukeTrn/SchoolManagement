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

        public ClassService(ILogger<ClassService> logger, SchoolManagementDbContext context, IEntityFilterService<ClassEntity> filterBuilder)
        {
            _logger = logger;
            _context = context;
            _filterBuilder = filterBuilder;
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
                    .OrderByDescending(c => c.ClassId)
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
                        // Thêm các trường khác cần thiết vào đây nếu cần
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
        ///  filter: get HR teacher (bug: need to add academicYear to sort out teachers who already chủ nhiệm in 1 niên khóa (done)
        /// </summary>
        /// <param name="grade"></param>
        /// <returns></returns>
        public async ValueTask<IEnumerable<TeacherFilterModel>> GetAvailableTeachersByGradeAsync(int grade, string academicYear)
        {
            try
            {
                _logger.LogInformation($"Starting to filter available teachers for grade {grade} and academic year {academicYear}.");

                // Lấy danh sách giáo viên chưa chủ nhiệm lớp nào trong khối chỉ định và niên khóa chỉ định
                var availableTeachers = await _context.TeacherEntities
                    .Where(t => !t.Classes.Any(c => c.Grade == grade && c.AcademicYear == academicYear))
                    .Select(t => new TeacherFilterModel
                    {
                        TeacherId = t.TeacherId,
                        FullName = t.FullName
                    })
                    .ToListAsync();

                _logger.LogInformation($"Found {availableTeachers.Count} available teachers for grade {grade} and academic year {academicYear}.");

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
                _logger.LogInformation("Start to create new class in {grade}.",model.Grade);
                // Logic for academic year
                //var prefixClassId = GenerateClassId(model.ClassName, model.Semester, model.AcademicYear);
                var prefixClassId = GenerateClassId(model.ClassName, model.AcademicYear);
                var existClass = await _context.ClassEntities.FirstOrDefaultAsync(s => s.ClassId == prefixClassId && s.Grade == model.Grade);
                if (existClass != null)
                {
                    _logger.LogInformation("Class ID has already existed");
                    throw ExistRecordException.ExistsRecord("Class ID already exists");
                }

                // Kiểm tra xem HomeroomTeacherId đã được sử dụng ở lớp khác chưa
                var isHomeroomTeacherInUse = await _context.ClassEntities
                    .AnyAsync(c => c.HomeroomTeacherId == model.HomeroomTeacherId && c.Grade == model.Grade && c.AcademicYear == model.AcademicYear);

                if (isHomeroomTeacherInUse)
                {
                    _logger.LogInformation("Homeroom teacher with ID {HomeroomTeacherId} is already assigned to another class.", model.HomeroomTeacherId);
                    throw new InvalidOperationException($"Homeroom teacher with ID {model.HomeroomTeacherId} is already assigned to another class");
                }

                var newClass = new ClassEntity()
                {
                    ClassId = prefixClassId,
                    ClassName = model.ClassName,
                    AcademicYear = model.AcademicYear,
                    Grade = model.Grade,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = null,
                    HomeroomTeacherId = model.HomeroomTeacherId,
                };

                _context.ClassEntities.Add(newClass);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while creating new Class. Error: {ex}", ex.Message);
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
        public async ValueTask UpdateClass(string classId, ClassUpdateModel model)
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

                var teacherAlreadyAssigned = await _context.ClassEntities
                    .AnyAsync(c => c.HomeroomTeacherId == model.HoomroomTeacherId && c.ClassId != classId);
                if (teacherAlreadyAssigned)
                {
                    _logger.LogWarning("Homeroom teacher with ID {HomeroomTeacherId} is already assigned to another class.", model.HoomroomTeacherId);
                    throw new InvalidOperationException("Homeroom teacher is already assigned to another class.");
                }

                existingClass.ClassName = model.ClassName;
                existingClass.HomeroomTeacherId = model.HoomroomTeacherId;
                existingClass.ModifiedAt = DateTime.Now;

                _context.ClassEntities.Update(existingClass);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Class with ID {ClassId} updated successfully.", classId);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while updating class with ID {ClassId}. Error: {Error}", classId, ex.Message);
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
            // Tách năm bắt đầu và năm kết thúc từ niên khóa
            var years = academicYear.Split(" - ");
            if (years.Length != 2)
            {
                throw new ArgumentException("Invalid academic year format. Expected format: 'YYYY - YYYY'.");
            }

            var startYear = years[0];
            var endYear = years[1];

            // Kết hợp các thành phần thành chuỗi kết quả
            var result = $"{startYear}{endYear}{className}";

            return result;
        }

    }
}
