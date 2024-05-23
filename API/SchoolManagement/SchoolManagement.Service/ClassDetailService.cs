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



        //public async Task AddClassDetails(List<ClassDetailAddModel> models)
        //{
        //    try
        //    {
        //        // Kiểm tra xem danh sách models có rỗng hay không
        //        if (models == null || !models.Any())
        //        {
        //            _logger.LogWarning("No class details provided.");
        //        }

        //        foreach (var model in models)
        //        {
        //            // Kiểm tra xem lớp học và sinh viên có tồn tại không
        //            var existingClass = await _context.ClassEntities.FindAsync(model.ClassId);
        //            var existingStudent = await _context.StudentEntities.FindAsync(model.StudentId);
        //            if (existingClass == null || existingStudent == null)
        //            {
        //                _logger.LogWarning("Class or student not found for ClassId: {ClassId}, StudentId: {StudentId}", model.ClassId, model.StudentId);
        //            }
        //        }

        //        // Lấy danh sách chi tiết lớp học hiện có
        //        //var classDetails = await _context.ClassDetailEntities.Include(cd => cd.Student).ToListAsync();
        //        List<ClassDetailEntity> entities = new List<ClassDetailEntity>();
        //        // Thêm chi tiết lớp học mới từ danh sách models
        //        foreach (var model in models)
        //        {
        //            var newClassDetail = new ClassDetailEntity
        //            {
        //                ClassDetailId = $"{model.ClassId}{model.StudentId}", // Tạo ClassDetailId từ ClassId và StudentId
        //                ClassId = model.ClassId,
        //                StudentId = model.StudentId,
        //                //Student = await _context.StudentEntities.FindAsync(model.StudentId) // Lấy thông tin sinh viên từ cơ sở dữ liệu
        //            };

        //            entities.Add(newClassDetail);
        //        }

        //        // Sắp xếp danh sách chi tiết lớp học theo thứ tự tên của học sinh
        //        //classDetails = OrderClassDetailsByName(classDetails);

        //        // Cập nhật lại số thứ tự cho từng chi tiết lớp học
        //        //UpdateClassDetailNumbers(classDetails);

        //        // Lưu thay đổi vào cơ sở dữ liệu
        //        await _context.SaveChangesAsync();

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("An error occurred while adding class details. Error: {Error}", ex.Message);
        //        throw;
        //    }
        //}

        public async Task AddClassDetails(List<ClassDetailAddModel> models)
        {
            try
            {
                foreach (var model in models)
                {
                    // Kiểm tra xem lớp học và học sinh có tồn tại không
                    var @class = await _context.ClassEntities.FirstOrDefaultAsync(c => c.ClassId == model.ClassId);
                    var student = await _context.StudentEntities.FirstOrDefaultAsync(s => s.StudentId == model.StudentId);

                    if (@class == null)
                    {
                        throw new Exception($"Lớp học với Id '{model.ClassId}' không tồn tại");
                    }

                    if (student == null)
                    {
                        throw new Exception($"Học sinh với Id '{model.StudentId}' không tồn tại");
                    }

                    // Tạo ClassDetailId từ ClassId và StudentId
                    string classDetailId = $"{model.ClassId}{model.StudentId}";

                    // Tạo mới một ClassDetailEntity
                    var classDetail = new ClassDetailEntity
                    {
                        ClassDetailId = classDetailId,
                        ClassId = model.ClassId,
                        StudentId = model.StudentId
                    };

                    // Thêm ClassDetail vào DbContext
                    _context.ClassDetailEntities.Add(classDetail);
                }

                // Lưu các thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ nếu có
                Console.WriteLine($"Lỗi khi thêm ClassDetails: {ex.Message}");
                throw;
            }
        }

        private List<ClassDetailEntity> OrderClassDetailsByName(List<ClassDetailEntity> classDetails)
        {
            return classDetails.OrderBy(cd => cd.Student.FullName).ToList();
        }

        private void UpdateClassDetailNumbers(List<ClassDetailEntity> classDetails)
        {
            for (int i = 0; i < classDetails.Count; i++)
            {
                classDetails[i].Number = i + 1;
            }
        }

    }
}