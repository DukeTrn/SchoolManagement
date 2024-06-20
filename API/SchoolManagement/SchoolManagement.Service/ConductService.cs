using MathNet.Numerics.Distributions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Common.Enum;
using SchoolManagement.Common.Exceptions;
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

        
        /// <summary>
        /// Second displays
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="semesterId"></param>
        /// <param name="classId"></param>
        /// <param name="queryModel"></param>
        /// <returns></returns>
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

        public async ValueTask<ConductInSemesterModel> GetConduct(string studentId, string semesterId)
        {
            try
            {
                _logger.LogInformation("Fetching conduct for student ID {StudentId} in semester ID {SemesterId}.", studentId, semesterId);

                var conductEntity = await _context.ConductEntities
                    .FirstOrDefaultAsync(c => c.StudentId == studentId && c.SemesterId == semesterId);

                if (conductEntity == null)
                {
                    await CreateConduct(studentId, semesterId);

                    // Re-query
                    conductEntity = await _context.ConductEntities
                        .FirstOrDefaultAsync(c => c.StudentId == studentId && c.SemesterId == semesterId);

                    if (conductEntity == null)
                    {
                        throw new InvalidOperationException("Failed to create or retrieve the conduct entity.");
                    }
                }

                var conductModel = new ConductInSemesterModel
                {
                    ConductId = conductEntity.ConductId,
                    ConductName = conductEntity.ConductName == ConductType.Null ? ConductType.Null : conductEntity.ConductName,
                    Feedback = conductEntity.Feedback == string.Empty ? "" : conductEntity.Feedback,
                };

                _logger.LogInformation("Successfully retrieved conduct for student ID {StudentId} in semester ID {SemesterId}.", studentId, semesterId);
                return conductModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while fetching conduct for student ID {StudentId} in semester ID {SemesterId}. Error: {Error}", studentId, semesterId, ex.Message);
                throw;
            }
        }

        public async ValueTask<ConductInSemesterModel> GetConductByClassDetailId(string classDetailId, string semesterId)
        {
            try
            {
                var classDetailExist = await _context.ClassDetailEntities.AnyAsync(s => s.ClassDetailId == classDetailId);
                if (!classDetailExist)
                {
                    var errorMsg = $"Không tìm thấy Class Detail ID {classDetailId} này!";
                    _logger.LogWarning(errorMsg);
                    throw new NotFoundException(errorMsg);
                }

                var semesterExist = await _context.SemesterEntities.AnyAsync(s => s.SemesterId == semesterId);
                if (!semesterExist)
                {
                    var errorMsg = $"Không tìm thấy mã học kì {semesterId} này!";
                    _logger.LogWarning(errorMsg);
                    throw new NotFoundException(errorMsg);
                }

                var conductEntity = await _context.ConductEntities
                    .Where(c => c.Student.ClassDetails.Any(cd => cd.ClassDetailId == classDetailId) && c.SemesterId == semesterId)
                    .FirstOrDefaultAsync();

                var student = await _context.StudentEntities
                    .Where(s => s.ClassDetails.Any(s => s.ClassDetailId == classDetailId))
                    .FirstOrDefaultAsync();

                if (conductEntity == null)
                {
                    //var errorMsg = $"Không tìm thấy thông tin hạnh kiểm cho Class Detail ID {classDetailId} trong học kì {semesterId} này!";
                    //_logger.LogWarning(errorMsg);
                    //throw new NotFoundException(errorMsg);
                    await CreateConduct(student.StudentId, semesterId);

                    // Re-query
                    conductEntity = await _context.ConductEntities
                        .FirstOrDefaultAsync(c => c.StudentId == student.StudentId && c.SemesterId == semesterId);

                    if (conductEntity == null)
                    {
                        throw new InvalidOperationException("Failed to create or retrieve the conduct entity.");
                    }
                }

                var conductInSemesterModel = new ConductInSemesterModel
                {
                    ConductId = conductEntity.ConductId,
                    ConductName = conductEntity.ConductName == ConductType.Null ? ConductType.Null : conductEntity.ConductName,
                    Feedback = conductEntity.Feedback == string.Empty ? "" : conductEntity.Feedback,
                };

                return conductInSemesterModel;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError("Not Found: {ex}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting the conduct information. Error: {ex}", ex.Message);
                throw;
            }
        }


        /// <summary>
        /// Auto create conduct for a student (currently not used for api)
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
                    ConductName = ConductType.Null,
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

        public async ValueTask DeleteConduct(Guid conductId)
        {
            try
            {
                _logger.LogInformation("Start deleting conduct with ID {id}", conductId);
                var conduct = await _context.ConductEntities.FindAsync(conductId);
                if (conduct == null)
                {
                    throw new NotFoundException($"Conduct with ID {conductId} not found.");
                }
                _context.ConductEntities.Remove(conduct);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while deleting Conduct with ID {id}. Error: {ex}", conductId, ex);
                throw;
            }
        }

        public async ValueTask<ConductForClassStatisticModel> GetConductClassStatistic(int grade, string semesterId, string classId)
        {
            try
            {
                // Lấy tổng số học sinh trong lớp
                var totalStudents = await _context.ClassDetailEntities
                    .CountAsync(cd => cd.ClassId == classId && cd.Class.Grade == grade);

                if (totalStudents == 0)
                {
                    return new ConductForClassStatisticModel
                    {
                        VeryGoodCount = 0,
                        VeryGoodPercentage = "0%",
                        GoodCount = 0,
                        GoodPercentage = "0%",
                        AverageCount = 0,
                        AveragePercentage = "0%",
                        WeakCount = 0,
                        WeakPercentage = "0%"
                    };
                }

                // Lấy danh sách học sinh trong lớp
                var studentIds = await _context.ClassDetailEntities
                    .Where(cd => cd.ClassId == classId && cd.Class.Grade == grade)
                    .Select(cd => cd.StudentId)
                    .ToListAsync();

                // Lấy thống kê số lượng học sinh có từng loại hạnh kiểm
                var statistics = await _context.ConductEntities
                    .Where(c => studentIds.Contains(c.StudentId) && c.SemesterId == semesterId)
                    .GroupBy(c => c.ConductName)
                    .Select(g => new
                    {
                        ConductName = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();

                var veryGoodCount = statistics.FirstOrDefault(s => s.ConductName == ConductType.VeryGood)?.Count ?? 0;
                var goodCount = statistics.FirstOrDefault(s => s.ConductName == ConductType.Good)?.Count ?? 0;
                var averageCount = statistics.FirstOrDefault(s => s.ConductName == ConductType.Average)?.Count ?? 0;
                var weakCount = statistics.FirstOrDefault(s => s.ConductName == ConductType.Weak)?.Count ?? 0;

                // Tính tỷ lệ phần trăm
                var result = new ConductForClassStatisticModel
                {
                    TotalStudent = totalStudents,
                    VeryGoodCount = veryGoodCount,
                    VeryGoodPercentage = $"{(veryGoodCount / (decimal)totalStudents * 100):F2}%",
                    GoodCount = goodCount,
                    GoodPercentage = $"{(goodCount / (decimal)totalStudents * 100):F2}%",
                    AverageCount = averageCount,
                    AveragePercentage = $"{(averageCount / (decimal)totalStudents * 100):F2}%",
                    WeakCount = weakCount,
                    WeakPercentage = $"{(weakCount / (decimal)totalStudents * 100):F2}%"
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting conduct statistics. Error: {ex}", ex.Message);
                throw;
            }
        }

        public async ValueTask<ConductForSemesterStatisticModel> GetConductSemesterStatistic(int grade, string semesterId)
        {
            try
            {
                // Lấy tổng số học sinh trong khối và học kỳ cụ thể
                var totalStudents = await _context.ClassDetailEntities
                    .CountAsync(cd => cd.Class.Grade == grade);

                if (totalStudents == 0)
                {
                    return new ConductForSemesterStatisticModel
                    {
                        VeryGoodPercentage = "0%",
                        GoodPercentage = "0%",
                        AveragePercentage = "0%",
                        WeakPercentage = "0%"
                    };
                }

                // Lấy danh sách StudentId của học sinh trong khối và học kỳ cụ thể
                var studentIds = await _context.ClassDetailEntities
                    .Where(cd => cd.Class.Grade == grade)
                    .Select(cd => cd.StudentId)
                    .ToListAsync();

                // Lấy thống kê số lượng học sinh có từng loại hạnh kiểm trong học kỳ cụ thể
                var conductStatistics = await _context.ConductEntities
                    .Where(c => studentIds.Contains(c.StudentId) && c.SemesterId == semesterId)
                    .GroupBy(c => c.ConductName)
                    .Select(g => new
                    {
                        ConductName = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();

                // Tính tỷ lệ phần trăm của từng loại hạnh kiểm
                var veryGoodCount = conductStatistics.FirstOrDefault(s => s.ConductName == ConductType.VeryGood)?.Count ?? 0;
                var goodCount = conductStatistics.FirstOrDefault(s => s.ConductName == ConductType.Good)?.Count ?? 0;
                var averageCount = conductStatistics.FirstOrDefault(s => s.ConductName == ConductType.Average)?.Count ?? 0;
                var weakCount = conductStatistics.FirstOrDefault(s => s.ConductName == ConductType.Weak)?.Count ?? 0;

                var result = new ConductForSemesterStatisticModel
                {
                    VeryGoodPercentage = $"{(veryGoodCount / (decimal)totalStudents * 100):F2}%",
                    GoodPercentage = $"{(goodCount / (decimal)totalStudents * 100):F2}%",
                    AveragePercentage = $"{(averageCount / (decimal)totalStudents * 100):F2}%",
                    WeakPercentage = $"{(weakCount / (decimal)totalStudents * 100):F2}%"
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting conduct statistics for semester. Error: {ex}", ex.Message);
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
