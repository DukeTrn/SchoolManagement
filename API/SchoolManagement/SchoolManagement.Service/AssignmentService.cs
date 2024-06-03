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
                    AssignmentId = a.AssignmentId,
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
        /// Create assignment (thêm logic check xem giáo viên đã ở trong lớp khác chưa)
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
                var semester = await _context.SemesterEntities.AsNoTracking().FirstOrDefaultAsync(s => s.SemesterId == model.SemesterId);
                var teacher = await _context.TeacherEntities.AsNoTracking().FirstOrDefaultAsync(t => t.TeacherId == model.TeacherId);
                var subject = await _context.SubjectEntities.AsNoTracking().FirstOrDefaultAsync(s => s.SubjectId == model.SubjectId);
                var classEntity = await _context.ClassEntities.AsNoTracking().FirstOrDefaultAsync(c => c.ClassId == model.ClassId);

                if (semester == null)
                {
                    _logger.LogWarning("Semester not found: {SemesterId}", model.SemesterId);
                    throw new KeyNotFoundException($"Semester not found: {model.SemesterId}");
                }
                if (teacher == null)
                {
                    _logger.LogWarning("Teacher not found: {TeacherId}", model.TeacherId);
                    throw new KeyNotFoundException($"Teacher not found: {model.TeacherId}");
                }
                if (subject == null)
                {
                    _logger.LogWarning("Subject not found: {SubjectId}", model.SubjectId);
                    throw new KeyNotFoundException($"Subject not found: {model.SubjectId}");
                }
                if (classEntity == null)
                {
                    _logger.LogWarning("Class not found: {ClassId}", model.ClassId);
                    throw new KeyNotFoundException($"Class not found: {model.ClassId}");
                }

                // Kiểm tra xem phân công giảng dạy đã tồn tại chưa
                var existingAssignment = await _context.AssignmentEntities
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.SemesterId == model.SemesterId &&
                                              a.TeacherId == model.TeacherId &&
                                              a.SubjectId == model.SubjectId &&
                                              a.ClassId == model.ClassId);
                if (existingAssignment != null)
                {
                    _logger.LogWarning("Assignment already exists for SemesterId: {SemesterId}, TeacherId: {TeacherId}, SubjectId: {SubjectId}, ClassId: {ClassId}",
                                        model.SemesterId, model.TeacherId, model.SubjectId, model.ClassId);
                    throw new InvalidOperationException("Assignment already exists.");
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
                _logger.LogError(ex, "An error occurred while creating a new assignment.");
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
