using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Database;
using SchoolManagement.Model;
using SchoolManagement.Service.Intention;

namespace SchoolManagement.Service
{
    public class ConductService : IConductService
    {
        private readonly ILogger<ConductService> _logger;
        private readonly SchoolManagementDbContext _context;
        
        public ConductService(ILogger<ConductService> _logger,
            SchoolManagementDbContext _context)
        {
            this._logger = _logger;
            this._context = _context;
        }

        public async ValueTask<IEnumerable<ConductDisplayModel>> GetListClassesInSemester(int grade, string semesterId, ConductQueryModel queryModel)
        {
            try
            {
                _logger.LogInformation("Start to get list of classes for semester {SemesterId} and grade {Grade}.",semesterId, grade);

                // Truy vấn để lấy danh sách các lớp học trong học kỳ và khối được chỉ định
                var query = _context.ClassEntities
                    .Include(c => c.HomeroomTeacher) // Include the related teacher entity
                    .Include(c => c.ClassDetails) // Include class details to count total students
                    .ThenInclude(cd => cd.Student) // Include student entity to join with Conducts
                    .ThenInclude(s => s.Conducts) // Include conducts to filter by semester
                    .Where(c => c.Grade == grade &&
                                c.ClassDetails.Any(cd => cd.Student.Conducts.Any(con => con.SemesterId == semesterId)))
                    .AsQueryable();

                // Tìm kiếm theo giá trị tìm kiếm
                if (!string.IsNullOrEmpty(queryModel.SearchValue))
                {
                    _logger.LogInformation("Add Search Value: {SearchValue}", queryModel.SearchValue);
                    query = query.Where(c => c.ClassName.Contains(queryModel.SearchValue) ||
                                             c.HomeroomTeacher.FullName.ToLower().Contains(queryModel.SearchValue.ToLower()));
                }

                var result = await query.Select(c => new ConductDisplayModel
                {
                    ClassName = c.ClassName,
                    AcademicYear = c.AcademicYear,
                    TotalStudents = c.ClassDetails.Count(cd => cd.Student.Conducts.Any(con => con.SemesterId == semesterId)),
                    HomeroomTeacherName = c.HomeroomTeacher.FullName
                }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting list of classes for semester {SemesterId}. Error: {ex}", semesterId, ex.Message);
                throw;
            }
        }

        public async ValueTask CreateCondcut(string studentId, string semesterId)
        {

        }
    }
}
