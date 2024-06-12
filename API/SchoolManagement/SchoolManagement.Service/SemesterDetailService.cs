using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Database;
using SchoolManagement.Entity;
using SchoolManagement.Model;
using SchoolManagement.Service.Intention;

namespace SchoolManagement.Service
{
    public class SemesterDetailService : ISemesterDetailService
    {
        private readonly ILogger<SemesterDetailService> _logger;
        private readonly SchoolManagementDbContext _context;

        public SemesterDetailService(ILogger<SemesterDetailService> logger, SchoolManagementDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async ValueTask<PaginationModel<SemesterDetailDisplayModel>> GetAllClassesInSem(string semId, SemesterDetailQueryModel queryModel)
        {
            try
            {
                _logger.LogInformation("Start to get list of classes for the semester.");

                int pageNumber = queryModel.PageNumber != null && queryModel.PageNumber.Value > 0 ? queryModel.PageNumber.Value : 1;
                int pageSize = queryModel.PageSize != null && queryModel.PageSize.Value > 0 ? queryModel.PageSize.Value : 10;

                var query = _context.SemesterDetailEntities
                        .Include(sd => sd.Class)
                            .ThenInclude(c => c.HomeroomTeacher)
                        .Include(sd => sd.Semester)
                        .Where(sd => sd.SemesterId == semId)
                        .AsQueryable();

                #region Filter
                if (queryModel.Grades.Any())
                {
                    _logger.LogInformation("Filter by grades: {Grades}", string.Join(", ", queryModel.Grades));
                    query = query.Where(sd => queryModel.Grades.Contains(sd.Class.Grade));
                }
                #endregion

                #region Search
                if (!string.IsNullOrEmpty(queryModel.SearchValue))
                {
                    _logger.LogInformation("Search value: {SearchValue}", queryModel.SearchValue);
                    query = query.Where(sd => sd.Class.ClassName.Contains(queryModel.SearchValue) ||
                                              sd.Class.HomeroomTeacher.FullName.Contains(queryModel.SearchValue));
                }
                #endregion

                var totalCount = await query.CountAsync();

                var paginatedData = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(sd => new SemesterDetailDisplayModel
                    {
                        Id = sd.Id,
                        ClassId = sd.ClassId,
                        ClassName = sd.Class.ClassName,
                        HomeroomTeacherId = sd.Class.HomeroomTeacherId,
                        HomeroomTeacherName = sd.Class.HomeroomTeacher.FullName,
                        Grade = sd.Class.Grade,
                        AcademicYear = sd.Class.AcademicYear
                    })
                    .ToListAsync();

                return new PaginationModel<SemesterDetailDisplayModel>
                {
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    DataList = paginatedData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting the list of classes for the semester. Error: {Error}", ex.Message);
                throw;
            }
        }

        public async ValueTask<IEnumerable<ClassFilterModel>> GetClassesForFilter(string semesterId)
        {
            try
            {
                // Kiểm tra xem semesterId có tồn tại trong cơ sở dữ liệu không
                var semesterExists = await _context.SemesterEntities.AnyAsync(s => s.SemesterId == semesterId);

                // Nếu semesterId không tồn tại, trả về danh sách rỗng
                if (!semesterExists)
                    return Enumerable.Empty<ClassFilterModel>();

                // Lấy danh sách các lớp chưa được thêm vào học kỳ và có 4 số đầu giống nhau
                var classesNotInSemester = await _context.ClassEntities
                    .Where(c => !_context.SemesterDetailEntities
                        .Any(sd => sd.ClassId == c.ClassId && sd.SemesterId == semesterId))
                    .Where(c => c.ClassId.StartsWith(semesterId.Substring(0, 4))) // Lọc theo điều kiện có 4 số đầu giống nhau
                    .Select(c => new ClassFilterModel
                    {
                        ClassId = c.ClassId,
                        ClassName = c.ClassName
                    })
                    .OrderByDescending(p => p.ClassId).ToListAsync();

                return classesNotInSemester;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while fetching classes for filter. Error: {Error}", ex.Message);
                throw;
            }
        }


        public async ValueTask CreateSemDetail(SemesterDetailAddModel model)
        {
            try
            {
                _logger.LogInformation("Start to create semester details.");

                // Kiểm tra xem semester có tồn tại không
                var semesterExists = await _context.SemesterEntities.AnyAsync(s => s.SemesterId == model.SemesterId);
                if (!semesterExists)
                {
                    _logger.LogInformation("Semester not found.");
                    throw new KeyNotFoundException("Semester not found.");
                }

                var semesterDetails = new List<SemesterDetailEntity>();

                foreach (var classId in model.ClassId)
                {
                    // Kiểm tra xem lớp học có tồn tại không
                    var classExists = await _context.ClassEntities.AnyAsync(c => c.ClassId == classId);
                    if (!classExists)
                    {
                        _logger.LogInformation("Class not found for ClassId: {ClassId}", classId);
                        throw new KeyNotFoundException($"Class not found for ClassId: {classId}");
                    }

                    // Kiểm tra xem sự kết hợp giữa semester và class có tồn tại không
                    var existingSemDetail = await _context.SemesterDetailEntities
                        .AnyAsync(sd => sd.SemesterId == model.SemesterId && sd.ClassId == classId);

                    if (existingSemDetail)
                    {
                        _logger.LogInformation("SemesterDetail already exists for SemesterId: {SemesterId} and ClassId: {ClassId}", model.SemesterId, classId);
                        throw new InvalidOperationException($"SemesterDetail already exists for SemesterId: {model.SemesterId} and ClassId: {classId}");
                    }

                    var semesterDetail = new SemesterDetailEntity
                    {
                        Id = Guid.NewGuid(),
                        SemesterId = model.SemesterId,
                        ClassId = classId
                    };

                    semesterDetails.Add(semesterDetail);
                }

                _context.SemesterDetailEntities.AddRange(semesterDetails);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully created semester details.");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating semester details. Error: {Error}", ex.Message);
                throw;
            }
        }

        public async ValueTask DeleteSemesterDetail(List<Guid> ids)
        {
            try
            {
                _logger.LogInformation("Start to delete semester details.");

                // Lấy danh sách các SemesterDetailEntities cần xóa
                var semesterDetails = await _context.SemesterDetailEntities
                    .Where(sd => ids.Contains(sd.Id))
                    .ToListAsync();

                if (!semesterDetails.Any())
                {
                    _logger.LogInformation("No semester details found for the given IDs.");
                    throw new KeyNotFoundException("No semester details found for the given IDs.");
                }

                _context.SemesterDetailEntities.RemoveRange(semesterDetails);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted semester details.");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while deleting semester details. Error: {Error}", ex.Message);
                throw;
            }
        }

    }
}
