using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Common.Enum;
using SchoolManagement.Database;
using SchoolManagement.Entity;
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
                // Kiểm tra xem semesterId có tồn tại trong cơ sở dữ liệu không
                var semesterExists = await _context.SemesterEntities.AnyAsync(s => s.SemesterId == semesterId);

                // Nếu semesterId không tồn tại, trả về danh sách rỗng
                if (!semesterExists)
                    return Enumerable.Empty<ConductDisplayModel>();

                var classesInSemester = await _context.ClassEntities
                    .Where(c => c.Grade == grade)
                    .Where(c => _context.SemesterDetailEntities
                        .Any(sd => sd.ClassId == c.ClassId && sd.SemesterId == semesterId))
                    .Select(c => new ConductDisplayModel
                    {
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

        public async ValueTask<PaginationModel<ConductFullDetailModel>> GetClassStudentsWithConducts(int grade, string semesterId, string classId, PageQueryModel queryModel)
        {
            try
            {
                int pageNumber = queryModel.PageNumber != null && queryModel.PageNumber.Value > 0 ? queryModel.PageNumber.Value : 1;
                int pageSize = queryModel.PageSize != null && queryModel.PageSize.Value > 0 ? queryModel.PageSize.Value : 10;
                // Lấy danh sách sinh viên trong lớp cụ thể
                var classStudents = await _context.ClassDetailEntities
                    .Where(cd => cd.ClassId == classId)
                    .Select(cd => new ConductFullDetailModel
                    {
                        ClassDetailId = cd.ClassDetailId,
                        StudentId = cd.StudentId,
                        StudentName = cd.Student.FullName,
                    })
                    .ToListAsync();

                // Duyệt qua từng sinh viên và kiểm tra xem họ đã có conduct chưa
                foreach (var student in classStudents)
                {
                    // Kiểm tra xem sinh viên đã có conduct trong học kỳ này chưa
                    var existingConduct = await _context.ConductEntities
                        .FirstOrDefaultAsync(c => c.StudentId == student.StudentId && c.SemesterId == semesterId);

                    if (existingConduct == null)
                    {
                        // Nếu không có conduct, tạo một conduct mới
                        var conductId = await CreateConduct(student.StudentId, semesterId);
                        student.ConductId = conductId;
                    }
                    else
                    {
                        // Nếu có conduct, gán ConductId từ conduct đã tồn tại
                        student.ConductId = existingConduct.ConductId;
                        student.ConductName = TranslateStatus(existingConduct.ConductName); // Chuyển Enum sang string
                        student.Feedback = existingConduct.Feedback;
                    }
                }

                // Áp dụng phân trang
                var paginatedResults = classStudents
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

                // Tính tổng số lượng sinh viên trong lớp
                var totalStudents = await _context.ClassDetailEntities
                    .Where(cd => cd.ClassId == classId)
                    .CountAsync();

                return new PaginationModel<ConductFullDetailModel>
                {
                    TotalCount = totalStudents,
                    PageNumber = queryModel.PageNumber,
                    PageSize = queryModel.PageSize,
                    DataList = paginatedResults,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting class students with conducts. Error: {ex}", ex.Message);
                throw;
            }
        }



        /// <summary>
        /// Auto create conduct for a student
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="semesterId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async ValueTask<Guid> CreateConduct(string studentId, string semesterId)
        {
            try
            {
                // Kiểm tra xem semesterId và studentId có tồn tại trong cơ sở dữ liệu không
                var semesterExists = await _context.SemesterEntities.AnyAsync(s => s.SemesterId == semesterId);
                var studentExists = await _context.StudentEntities.AnyAsync(s => s.StudentId == studentId);

                // Nếu semesterId hoặc studentId không tồn tại, throw exception
                if (!semesterExists || !studentExists)
                {
                    throw new ArgumentException("SemesterId or StudentId not found.");
                }

                // Kiểm tra xem conduct đã tồn tại cho cặp studentId và semesterId chưa
                var conductExists = await _context.ConductEntities.AnyAsync(c => c.StudentId == studentId && c.SemesterId == semesterId);

                if (conductExists)
                {
                    throw new InvalidOperationException("Conduct already exists for the given student and semester.");
                }

                // Tạo mới đối tượng ConductEntity và thêm vào DbContext
                var conduct = new ConductEntity
                {
                    ConductId = Guid.NewGuid(), // Tạo mới conductId
                    StudentId = studentId,
                    SemesterId = semesterId
                    // Thêm các trường khác nếu cần
                };

                _context.ConductEntities.Add(conduct);
                await _context.SaveChangesAsync();

                return conduct.ConductId;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating conduct. Error: {Error}", ex.Message);
                throw;
            }
        }

        public async ValueTask UpdateConduct(Guid conductId, ConductUpdateModel model)
        {
            try
            {
                // Tìm conduct dựa trên conductId
                var conduct = await _context.ConductEntities.FindAsync(conductId);

                // Kiểm tra nếu conduct không tồn tại
                if (conduct == null)
                {
                    throw new KeyNotFoundException($"Conduct with ID {conductId} not found.");
                }

                // Cập nhật thông tin conduct
                conduct.ConductName = model.ConductType;
                conduct.Feedback = model.Feedback ?? "";

                // Lưu các thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while updating conduct. Error: {ex}", ex.Message);
                throw;
            }
        }


        private static string TranslateStatus(ConductType status)
        {
            switch (status)
            {
                case ConductType.VeryGood:
                    return "Tốt";
                case ConductType.Good:
                    return "Khá";
                case ConductType.Average:
                    return "Trung bình";
                case ConductType.Weak:
                    return "Yếu";
                default:
                    return string.Empty;
            }
        }
    }
}
