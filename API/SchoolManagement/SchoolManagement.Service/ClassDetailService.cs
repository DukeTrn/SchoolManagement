using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Database;
using SchoolManagement.Entity;
using SchoolManagement.Model.ClassDetail;
using SchoolManagement.Service.Intention;

namespace SchoolManagement.Service
{
    public class ClassDetailService : IClassDetailService
    {
        private readonly ILogger<ClassDetailService> _logger;
        private readonly SchoolManagementDbContext _context;

        public ClassDetailService(ILogger<ClassDetailService> logger, SchoolManagementDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async ValueTask AddClassDetails(List<ClassDetailAddModel> models)
        {
            try
            {
                _logger.LogInformation("Start to add class details.");

                // Lặp qua danh sách StudentClassModel và thêm các chi tiết lớp học tương ứng
                foreach (var model in models)
                {
                    // Kiểm tra xem lớp học có tồn tại không
                    var existingClass = await _context.ClassEntities.FirstOrDefaultAsync(c => c.ClassId == model.ClassId);
                    if (existingClass == null)
                    {
                        _logger.LogWarning("Class with ID {ClassId} not found.", model.ClassId);
                        throw new NotFoundException("Class not found.");
                    }

                    // Kiểm tra xem sinh viên có tồn tại không
                    var existingStudent = await _context.StudentEntities.FirstOrDefaultAsync(s => s.StudentId == model.StudentId);
                    if (existingStudent == null)
                    {
                        _logger.LogWarning("Student with ID {StudentId} not found.", model.StudentId);
                        throw new NotFoundException("Student not found.");
                    }

                    // Kiểm tra xem chi tiết lớp học đã tồn tại chưa
                    var existingClassDetail = await _context.ClassDetailEntities.FirstOrDefaultAsync(cd => cd.ClassId == model.ClassId && cd.StudentId == model.StudentId);
                    if (existingClassDetail != null)
                    {
                        _logger.LogWarning("Class detail for student {StudentId} in class {ClassId} already exists.", model.StudentId, model.ClassId);
                        continue; // Đã tồn tại, tiếp tục với sinh viên tiếp theo
                    }

                    // Tạo chi tiết lớp học mới và thêm vào cơ sở dữ liệu
                    var classDetail = new ClassDetailEntity
                    {
                        ClassId = model.ClassId,
                        StudentId = model.StudentId
                    };
                    _context.ClassDetailEntities.Add(classDetail);
                }

                // Lưu các thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();

                _logger.LogInformation("Class details added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while adding class details. Error: {Error}", ex.Message);
                throw;
            }
        }
    }
}