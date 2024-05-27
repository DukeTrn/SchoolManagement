using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Database;
using SchoolManagement.Entity;
using SchoolManagement.Model;
using SchoolManagement.Service.Intention;

namespace SchoolManagement.Service
{
    public class AssignmentService : IAssignmentService
    {
        private readonly ILogger<ClassDetailService> _logger;
        private readonly SchoolManagementDbContext _context;

        public AssignmentService(ILogger<ClassDetailService> logger, SchoolManagementDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Get list assignments by grade, subject id, semester id
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="semesterId"></param>
        /// <param name="subjectId"></param>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public async ValueTask<IEnumerable<AssignmentDisplayModel>> GetListAssignments(int grade, string semesterId, int subjectId ,AssignmentQueryModel queryModel)
        {
            try
            {
                _logger.LogInformation("Start to get list of assignments for grade {Grade} and semester {SemesterId}.", grade, semesterId);

                // Truy vấn để lấy danh sách các phân công giảng dạy theo khối và học kỳ
                var query = _context.AssignmentEntities
                    .Include(a => a.Teacher)
                    .Include(a => a.Class)
                    .Include(a => a.Semester)
                    .Where(a => a.Class.Grade == grade && a.SemesterId == semesterId && a.SubjectId == subjectId)
                    .AsQueryable();

                #region Search
                // Thêm chức năng tìm kiếm
                if (!string.IsNullOrEmpty(queryModel.SearchValue))
                {
                    var searchValue = queryModel.SearchValue.ToLower();
                    query = query.Where(a =>
                        a.Teacher.FullName.ToLower().Contains(searchValue) ||
                        a.Teacher.Email.ToLower().Contains(searchValue) ||
                        a.Teacher.PhoneNumber.ToLower().Contains(searchValue) ||
                        a.Class.ClassName.ToLower().Contains(searchValue));
                }
                #endregion
                #region Filter
                // Lọc theo danh sách ClassIds nếu có
                //if (queryModel.ClassIds.Any())
                //{
                //    query = query.Where(a => queryModel.ClassIds.Contains(a.ClassId));
                //}
                #endregion

                var result = await query.Select(a => new AssignmentDisplayModel
                {
                    TeacherId = a.TeacherId,
                    TeacherName = a.Teacher.FullName,
                    Email = a.Teacher.Email,
                    PhoneNumber = a.Teacher.PhoneNumber,
                    ClassName = a.Class.ClassName,
                    SemesterName = a.Semester.SemesterName, // Assuming there is a SemesterName property in SemesterEntity
                    AcademicYear = a.Class.AcademicYear
                }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting list of assignments for grade {Grade} and semester {SemesterId}. Error: {ex}", grade, semesterId, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Create assignment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async ValueTask CreateAssignment(AssignmentAddModel model)
        {
            try
            {
                _logger.LogInformation("Start to create a new assignment.");

                // Kiểm tra xem học kỳ, giáo viên, môn học và lớp học có tồn tại không
                var semester = await _context.SemesterEntities.FindAsync(model.SemesterId);
                var teacher = await _context.TeacherEntities.FindAsync(model.TeacherId);
                var subject = await _context.SubjectEntities.FindAsync(model.SubjectId);
                var classEntity = await _context.ClassEntities.FindAsync(model.ClassId);

                if (semester == null || teacher == null || subject == null || classEntity == null)
                {
                    _logger.LogWarning("Semester, teacher, subject, or class not found.");
                    throw new KeyNotFoundException("Semester, teacher, subject, or class not found.");
                }

                // Tạo một phân công giảng dạy mới
                var assignment = new AssignmentEntity
                {
                    SemesterId = model.SemesterId,
                    TeacherId = model.TeacherId,
                    SubjectId = model.SubjectId,
                    ClassId = model.ClassId
                };

                // Thêm phân công giảng dạy vào DbContext và lưu vào cơ sở dữ liệu
                _context.AssignmentEntities.Add(assignment);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully created a new assignment.");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating a new assignment. Error: {ex}", ex);
                throw;
            }
        }

        /// <summary>
        /// Update assignment (phân công chuyển lớp cho giáo viên)
        /// </summary>
        /// <param name="assignmentId"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async ValueTask UpdateAssignment(Guid assignmentId, string teacherId)
        {
            try
            {
                _logger.LogInformation("Start to update assignment.");

                // Tìm phân công giảng dạy cần cập nhật
                var assignment = await _context.AssignmentEntities.FindAsync(assignmentId);

                if (assignment == null)
                {
                    _logger.LogWarning("Assignment not found.");
                    throw new KeyNotFoundException("Assignment not found.");
                }

                // Kiểm tra xem giáo viên mới có tồn tại không
                var newTeacher = await _context.TeacherEntities.FindAsync(teacherId);

                if (newTeacher == null)
                {
                    _logger.LogWarning("New teacher not found.");
                    throw new KeyNotFoundException("New teacher not found.");
                }

                // Cập nhật giáo viên cho phân công giảng dạy
                assignment.TeacherId = teacherId;

                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully updated assignment.");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while updating assignment. Error: {ex}", ex);
                throw;
            }
        }

        public async ValueTask DeleteAssignment(Guid assignmentId)
        {
            try
            {
                // Tìm phân công giảng dạy cần xóa trong cơ sở dữ liệu
                var assignment = await _context.AssignmentEntities.FindAsync(assignmentId);

                // Nếu không tìm thấy phân công giảng dạy, ném ngoại lệ hoặc xử lý theo nhu cầu
                if (assignment == null)
                {
                    _logger.LogWarning($"Assignment with ID {assignmentId} not found.");
                    throw new KeyNotFoundException($"Assignment with ID {assignmentId} not found.");
                }

                // Xóa phân công giảng dạy từ cơ sở dữ liệu và lưu thay đổi
                _context.AssignmentEntities.Remove(assignment);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Assignment with ID {assignmentId} deleted successfully.");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và log thông báo lỗi nếu có
                _logger.LogError($"An error occurred while deleting assignment: {ex.Message}");
                throw;
            }
        }

    }
}
