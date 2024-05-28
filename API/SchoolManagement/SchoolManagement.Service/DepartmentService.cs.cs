using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Database;
using SchoolManagement.Entity;
using SchoolManagement.Model;
using SchoolManagement.Service.Intention;
using SchoolManagement.Share;

namespace SchoolManagement.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ILogger<DepartmentEntity> _logger;
        private readonly SchoolManagementDbContext _context;

        public DepartmentService(ILogger<DepartmentEntity> logger,
            SchoolManagementDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        #region List departments
        /// <summary>
        /// Get list of all departments
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public async ValueTask<PaginationModel<DepartmentDisplayModel>> GetAllDepartments(PageModel queryModel)
        {
            try
            {
                _logger.LogInformation("Start to get list departments.");
                int pageNumber = queryModel.PageNumber != null && queryModel.PageNumber.Value > 0 ? queryModel.PageNumber.Value : 1;
                int pageSize = queryModel.PageSize != null && queryModel.PageSize.Value > 0 ? queryModel.PageSize.Value : 10;

                var query = _context.DepartmentEntities.AsQueryable();

                var paginatedData = await query
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
                _logger.LogInformation("Success to get list departments.");
                return new PaginationModel<DepartmentDisplayModel>
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
                _logger.LogError("An error occured while getting list of all departments. Error: {ex}", ex);
                throw;
            }
        }

        public async ValueTask<IEnumerable<DepartmentFilterModel>> DepartmentFilter()
        {
            try
            {
                return await _context.DepartmentEntities
                    .Select(c => new DepartmentFilterModel
                    {
                        DepartmentId = c.DepartmentId,
                        SubjectName = c.SubjectName
                    }).ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError("An error occured while getting list of all depts. Error: {ex}", ex);
                throw;
            }
        }
        #endregion

        #region Create
        /// <summary>
        /// Add new department
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async ValueTask CreateDepartment(DepartmentAddModel model)
        {
            try
            {
                _logger.LogInformation("Start to create new department.");
                var existingDept = await _context.DepartmentEntities.FirstOrDefaultAsync(s => s.DepartmentId == model.DepartmentId);
                if (existingDept != null)
                {
                    _logger.LogInformation("Department has already existed");
                    throw ExistRecordException.ExistsRecord("Department ID already exists");
                }

                var newDept = new DepartmentEntity()
                {
                    DepartmentId = model.DepartmentId,
                    SubjectName = model.SubjectName,
                    Description = model.Description,
                    Notification = model.Notification,
                };

                _context.DepartmentEntities.Add(newDept);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while creating new department. Error: {ex}", ex);
                throw;
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Update a department by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async ValueTask UpdateDepartment(string id, DepartmentUpdateModel model)
        {
            try
            {
                _logger.LogInformation("Start to update a department.");
                var dept = await _context.DepartmentEntities.FirstOrDefaultAsync(s => s.DepartmentId == id);
                if (dept == null)
                {
                    throw new NotFoundException($"Department with ID {id} not found.");
                }

                dept.SubjectName = model.SubjectName;
                dept.Description = model.Description;
                dept.Notification = model.Notification;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while updating a department. Error: {ex}", ex);
                throw;
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete dept by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async ValueTask DeleteDepartment(string id)
        {
            try
            {
                _logger.LogInformation("Start deleting department with ID {id}", id);

                var dept = await _context.DepartmentEntities.FirstOrDefaultAsync(s => s.DepartmentId == id);

                if (dept == null)
                {
                    throw new NotFoundException($"Department with ID {id} not found.");
                }

                _context.DepartmentEntities.Remove(dept);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Department with ID {id} deleted successfully.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while deleting Department with ID {id}. Error: {ex}", id, ex);
                throw;
            }
        }
        #endregion

        #region Update department ID of teachers (add to 1 dept or delete from 1 dept)

        public async ValueTask AddTeachersToDepartment(UpdateTeachersToDeptModel model)
        {
            try
            {
                // There will be a bug:
                // if a teacher is already in 1 department, then add again into the same department
                // => the result is still true (supposed to be false)
                // this bug will be handled by UI (related to filter)

                _logger.LogInformation("Start to add teachers to department.");

                // Kiểm tra xem tổ bộ môn có tồn tại không
                var department = await _context.DepartmentEntities
                    .Include(d => d.Teachers)
                    .FirstOrDefaultAsync(d => d.DepartmentId == model.DepartmentId);

                if (department == null)
                {
                    _logger.LogInformation("Department not found.");
                    throw new KeyNotFoundException("Không tìm thấy tổ bộ môn này!");
                }

                // Tìm các giáo viên có mã trong danh sách TeacherIds
                var teachers = await _context.TeacherEntities
                    .Where(t => model.TeacherIds.Contains(t.TeacherId))
                    .ToListAsync();

                if (!teachers.Any())
                {
                    _logger.LogInformation("No teachers found for the given IDs.");
                    throw new KeyNotFoundException("Không tìm thấy giáo viên này!");
                }

                var existingTeachers = new List<string>();
                var addedTeachers = new List<string>();

                // Thêm giáo viên vào tổ bộ môn
                foreach (var teacher in teachers)
                {
                    // Kiểm tra xem giáo viên đã thuộc tổ bộ môn nào chưa
                    if (teacher.DepartmentId != null && teacher.DepartmentId != model.DepartmentId)
                    {
                        existingTeachers.Add(teacher.TeacherId);
                    }
                    else
                    {
                        if (teacher.DepartmentId != model.DepartmentId)
                        {
                            teacher.DepartmentId = model.DepartmentId;
                            department.Teachers.Add(teacher);
                            addedTeachers.Add(teacher.TeacherId);
                        }
                    }
                }

                if (existingTeachers.Any())
                {
                    var errorMsg = $"The following teachers are already in a department: {string.Join(", ", existingTeachers)}";
                    _logger.LogInformation(errorMsg);
                    throw new InvalidOperationException(errorMsg);
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Successfully added teachers to department. Added Teachers: {string.Join(", ", addedTeachers)}");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while adding teachers to department. Error: {ex}", ex);
                throw;
            }
        }

        public async ValueTask RemoveTeachersFromDepartment(UpdateTeachersToDeptModel model)
        {
            try
            {
                _logger.LogInformation("Start to remove teachers from department.");

                // Kiểm tra xem tổ bộ môn có tồn tại không
                var department = await _context.DepartmentEntities
                    .Include(d => d.Teachers)
                    .FirstOrDefaultAsync(d => d.DepartmentId == model.DepartmentId);

                if (department == null)
                {
                    _logger.LogInformation("Department not found.");
                    throw new KeyNotFoundException("Department not found.");
                }

                // Tìm các giáo viên có mã trong danh sách TeacherIds
                var teachers = await _context.TeacherEntities
                    .Where(t => model.TeacherIds.Contains(t.TeacherId) && t.DepartmentId == model.DepartmentId)
                    .ToListAsync();

                if (!teachers.Any())
                {
                    _logger.LogInformation("No teachers found in this department.");
                    throw new KeyNotFoundException("No teachers found in this department.");
                }

                // Xóa giáo viên khỏi tổ bộ môn
                foreach (var teacher in teachers)
                {
                    teacher.DepartmentId = null;
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Successfully removed {teachers.Count} teachers from department {model.DepartmentId}.");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while removing teachers from department. Error: {ex}", ex);
                throw;
            }
        }

        public async ValueTask<IEnumerable<TeacherFilterModel>> GetFilterTeachersNotInAnyDepartment()
        {
            try
            {
                // Lấy danh sách tất cả giáo viên
                var allTeachers = await _context.TeacherEntities.ToListAsync();

                // Lọc danh sách giáo viên: chỉ lấy những giáo viên chưa thuộc bộ môn nào
                var teachersNotInAnyDepartment = allTeachers.Where(t => t.DepartmentId == null);

                // Chuyển đổi danh sách giáo viên thành danh sách TeacherFilterModel
                var teacherFilterModels = teachersNotInAnyDepartment.Select(t => new TeacherFilterModel
                {
                    TeacherId = t.TeacherId,
                    FullName = t.FullName
                }).ToList();

                return teacherFilterModels;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting teachers not in any department for filter. Error: {ex}", ex);
                throw;
            }
        }
        #endregion

        #region Promote
        /// <summary>
        /// Promote teachers by Ids (will enhance logic: choose 1 teacher for head and deputy)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async ValueTask PromoteTeachersAsync(PromoteTeacherModel model)
        {
            try
            {
                _logger.LogInformation("Start to promote teachers.");

                // Lấy tất cả giáo viên trong bộ môn
                var departmentTeachers = await _context.TeacherEntities
                    .Where(t => t.DepartmentId == model.DepartmentId)
                    .ToListAsync();

                // Lấy giáo viên hiện đang nắm giữ các vai trò
                var currentHead = departmentTeachers.Find(t => t.Role == Common.Enum.TeacherRole.Head);
                var currentDeputies = departmentTeachers.Where(t => t.Role == Common.Enum.TeacherRole.DeputyHead).ToList();

                // Giáng chức tất cả giáo viên trong bộ môn về vai trò "Giáo viên"
                foreach (var teacher in departmentTeachers)
                {
                    teacher.Role = Common.Enum.TeacherRole.Regular;
                }

                // Thăng chức giáo viên thành "Trưởng bộ môn"
                if (!string.IsNullOrEmpty(model.HeadId))
                {
                    var headTeacher = departmentTeachers.Find(t => t.TeacherId == model.HeadId);
                    if (headTeacher != null)
                    {
                        headTeacher.Role = Common.Enum.TeacherRole.Head;
                    }
                }

                // Thăng chức giáo viên thành "Phó bộ môn 1"
                if (!string.IsNullOrEmpty(model.FirstDeputyId))
                {
                    var deputy1Teacher = departmentTeachers.Find(t => t.TeacherId == model.FirstDeputyId);
                    if (deputy1Teacher != null)
                    {
                        deputy1Teacher.Role = Common.Enum.TeacherRole.DeputyHead;
                    }
                }

                // Thăng chức giáo viên thành "Phó bộ môn 2"
                if (!string.IsNullOrEmpty(model.SecondDeputyId))
                {
                    var deputy2Teacher = departmentTeachers.Find(t => t.TeacherId == model.SecondDeputyId);
                    if (deputy2Teacher != null)
                    {
                        deputy2Teacher.Role = Common.Enum.TeacherRole.DeputyHead;
                    }
                }

                // Kiểm tra và giáng chức các giáo viên hiện đang là phó bộ môn nhưng không được chọn
                foreach (var deputy in currentDeputies)
                {
                    if (deputy.TeacherId != model.FirstDeputyId && deputy.TeacherId != model.SecondDeputyId)
                    {
                        deputy.Role = Common.Enum.TeacherRole.Regular;
                    }
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully promoted teachers.");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while promoting teachers. Error: {ex}", ex);
                throw;
            }
        }

        #endregion

    }
}
