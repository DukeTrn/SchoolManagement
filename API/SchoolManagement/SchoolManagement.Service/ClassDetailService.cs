using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Common.Enum;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Database;
using SchoolManagement.Entity;
using SchoolManagement.Model;
using SchoolManagement.Model.ClassDetail;
using SchoolManagement.Service.Intention;
using System.Data;

namespace SchoolManagement.Service
{
    public class ClassDetailService : IClassDetailService
    {
        private readonly ILogger<ClassDetailService> _logger;
        private readonly SchoolManagementDbContext _context;
        private readonly IClassService _classService;
        private readonly IConductService _conductService;

        public ClassDetailService(ILogger<ClassDetailService> logger, 
            SchoolManagementDbContext context,
            IClassService classService,
            IConductService conductService)
        {
            _logger = logger;
            _context = context;
            _classService = classService;
            _conductService = conductService;
        }

        /// <summary>
        /// Get all students in 1 class (need to translate status)
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public async Task<PaginationModel<ClassDetailDisplayModel>> GetAllClassDetail(string classId, ClassDetailQueryModel queryModel)
        {
            try
            {
                int pageNumber = queryModel.PageNumber != null && queryModel.PageNumber.Value > 0 ? queryModel.PageNumber.Value : 1;
                int pageSize = queryModel.PageSize != null && queryModel.PageSize.Value > 0 ? queryModel.PageSize.Value : 10;

                // Lấy danh sách học sinh trong lớp theo classId và sắp xếp theo tên
                var query = _context.ClassDetailEntities
                    .Where(cd => cd.ClassId == classId)
                    .Include(cd => cd.Student) // Include Student entity
                    .Include(cd => cd.Class)
                    .OrderBy(cd => cd.Student.FullName)
                    .Select(cd => new ClassDetailDisplayModel
                    {
                        ClassDetailId = cd.ClassDetailId,
                        Grade = cd.Class.Grade,
                        Number = 0, // Sẽ được gán sau khi áp dụng phân trang
                        StudentId = cd.Student.StudentId,
                        FullName = cd.Student.FullName,
                        PhoneNumber = cd.Student.PhoneNumber,
                        Status = cd.Student.Status,
                    });


                #region Filter Status
                if (queryModel.Status.Any())
                {
                    query = query.Where(cd => queryModel.Status.Contains(cd.Status));
                }
                #endregion

                #region Search
                if (!string.IsNullOrEmpty(queryModel.SearchValue))
                {
                    _logger.LogInformation("Add Search Value: {SearchValue}", queryModel.SearchValue);
                    query = query.Where(c => c.FullName.ToLower().Contains(queryModel.SearchValue.ToLower()));
                }
                #endregion

                // Tổng số lượng học sinh trong lớp
                var totalCount = await query.CountAsync();

                // Áp dụng phân trang
                var paginatedData = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // Cập nhật lại Number theo thứ tự của trang hiện tại
                for (int i = 0; i < paginatedData.Count; i++)
                {
                    paginatedData[i].Number = (pageNumber - 1) * pageSize + i + 1;
                }

                return new PaginationModel<ClassDetailDisplayModel>
                {
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    DataList = paginatedData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting class details for classId {ClassId}. Error: {Error}", classId, ex.Message);
                throw;
            }
        }

        public async Task<List<StudentFilterModel>> FilterStudentsByGrade(string academicYear, int grade)
        {
            try
            {
                // Tạo chuỗi kiểm tra dựa trên năm bắt đầu của niên khóa
                var startYear = academicYear.Split(" - ")[0];

                // Lấy danh sách các studentIds đã tồn tại trong lớp học của khối grade và niên khóa chỉ định
                var existingStudentIds = await _context.ClassDetailEntities
                    .Where(cd => cd.Class.Grade == grade && cd.Class.AcademicYear == academicYear)
                    .Select(cd => cd.StudentId)
                    .ToListAsync();

                // Lọc danh sách học sinh không tồn tại trong các lớp đã lọc ở trên và bắt đầu bằng startYear
                var filteredStudents = await _context.StudentEntities
                    .Where(s => !existingStudentIds.Contains(s.StudentId) && s.StudentId.StartsWith(startYear))
                    .Select(s => new StudentFilterModel
                    {
                        StudentId = s.StudentId,
                        FullName = s.FullName
                    })
                    .ToListAsync();

                return filteredStudents;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while filtering students by class grade. Error: {ex.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// Create class details - Add students into class
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task AddClassDetails(List<ClassDetailAddModel> models)
        {
            var errors = new List<string>();

            try
            {
                var classDetails = new List<ClassDetailEntity>();

                foreach (var model in models)
                {
                    // Kiểm tra xem lớp học và học sinh có tồn tại không
                    var existingClass = await _context.ClassEntities.FindAsync(model.ClassId);
                    var existingStudent = await _context.StudentEntities.FindAsync(model.StudentId);
                    if (existingClass == null || existingStudent == null)
                    {
                        var errorMessage = $"Không tìm thấy Lớp hoặc Học sinh cho ClassId: {model.ClassId}, StudentId: {model.StudentId}";
                        _logger.LogWarning(errorMessage);
                        errors.Add(errorMessage);
                        continue;
                    }

                    // Kiểm tra xem học sinh đã thuộc lớp của cùng một khối chưa
                    var existingClassDetails = await _context.ClassDetailEntities
                        .Include(cd => cd.Class)
                        .Where(cd => cd.StudentId == model.StudentId && cd.Class.Grade == existingClass.Grade)
                        .ToListAsync();

                    if (existingClassDetails.Any())
                    {
                        var errorMessage = $"Học sinh với ID {model.StudentId} này đã học ở lớp khác trong khối {existingClass.Grade}.";
                        _logger.LogWarning(errorMessage);
                        errors.Add(errorMessage);
                        continue;
                    }

                    // Tạo ClassDetailId từ ClassId và StudentId
                    string classDetailId = $"{model.ClassId}{model.StudentId}";

                    // Tạo mới một ClassDetailEntity
                    var classDetail = new ClassDetailEntity
                    {
                        ClassDetailId = classDetailId,
                        ClassId = model.ClassId,
                        StudentId = model.StudentId,
                    };

                    // Thêm ClassDetail vào DbContext
                    classDetails.Add(classDetail);
                }

                // Thêm danh sách ClassDetail vào DbContext và lưu vào cơ sở dữ liệu
                if (classDetails.Any())
                {
                    _context.ClassDetailEntities.AddRange(classDetails);
                    await _context.SaveChangesAsync();
                }

                if (errors.Any())
                {
                    throw new Exception(string.Join("; ", errors));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating new Class Detail. Error: {ex}", ex.Message);
                throw;
            }
        }


        /// <summary>
        /// Update class Id (move a student to another class)
        /// </summary>
        /// <param name="classDetailId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public async ValueTask UpdateClassDetailAsync(string classDetailId, string newClassId)
        {
            try
            {
                _logger.LogInformation("Start to update classId {classId} of classDetailId {classDetailId}.", newClassId, classDetailId);

                var query = await _context.ClassDetailEntities.FindAsync(classDetailId);
                var existClsasId = await _context.ClassEntities.FindAsync(newClassId);
                if (query == null || existClsasId == null)
                {
                    _logger.LogWarning("ClassDetail or ClassId not found: {classDetailId}, StudentId: {newClassId}", classDetailId, newClassId);
                }
                if (query != null)
                {
                    query.ClassId = newClassId;
                    query.ModifiedAt = DateTime.Now;
                    await _context.SaveChangesAsync();
                }
                _logger.LogInformation("ClassId with ID {ClassId} updated successfully.", newClassId);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while updating classID with classDetailId {classDetailId}. Error: {Error}", classDetailId, ex.Message);
                throw;
            }
        }

        public async ValueTask DeleteClassDetail(string id)
        {
            try
            {
                _logger.LogInformation("Start to delete class detail with ID {id}.", id);

                var existingClassDetail = await _context.ClassDetailEntities.FirstOrDefaultAsync(c => c.ClassDetailId == id);
                if (existingClassDetail == null)
                {
                    _logger.LogWarning("Class detail with ID {id} not found.", id);
                    throw new NotFoundException("Class detail not found.");
                }

                _context.ClassDetailEntities.Remove(existingClassDetail);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Class detail with ID {id} deleted successfully.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while deleting class detail with ID {id}. Error: {Error}", id, ex.Message);
                throw;
            }
        }

        public async Task<byte[]> ExportClassDetailToExcelAsync(string classId, ClassDetailExportQueryModel queryModel)
        {
            try
            {
                List<ClassDetailEntity> classDetails;

                // Lọc dữ liệu theo class Id và các điều kiện khác từ queryModel
                var query = _context.ClassDetailEntities
                    .Include(cd => cd.Student)
                    .Where(cd => cd.ClassId == classId);

                // Lọc theo danh sách student Ids nếu có
                if (queryModel.StudentIds.Any())
                {
                    query = query.Where(cd => queryModel.StudentIds.Contains(cd.StudentId));
                }

                // Lọc theo danh sách trạng thái nếu có
                if (queryModel.Status.Any())
                {
                    query = query.Where(cd => queryModel.Status.Contains(cd.Student.Status));
                }

                // Lọc theo từ khóa tìm kiếm nếu có
                if (!string.IsNullOrEmpty(queryModel.SearchValue))
                {
                    query = query.Where(cd =>
                        cd.Student.FullName.Contains(queryModel.SearchValue) ||
                        cd.StudentId.Contains(queryModel.SearchValue));
                }

                // Sử dụng phân trang
                int pageNumber = queryModel.PageNumber != null && queryModel.PageNumber.Value > 0 ? queryModel.PageNumber.Value : 1;
                int pageSize = queryModel.PageSize != null && queryModel.PageSize.Value > 0 ? queryModel.PageSize.Value : 100;
                classDetails = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

                // Tạo DataTable và thêm dữ liệu chi tiết lớp vào đó
                DataTable dt = new DataTable();
                dt.Columns.Add("MSHS", typeof(string));
                dt.Columns.Add("Họ và tên", typeof(string));
                dt.Columns.Add("Tình trạng học tập", typeof(string));

                foreach (var cd in classDetails)
                {
                    dt.Rows.Add(cd.StudentId,
                        cd.Student?.FullName,
                        TranslateStatus(cd.Student.Status));
                }

                var className = await _classService.GetClassNameById(classId);
                // Xuất dữ liệu sang tệp Excel
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add($"Danh sách học sinh lớp {className}");
                    worksheet.Cell(1, 1).InsertTable(dt);

                    // Lưu tệp Excel vào một mảng byte[]
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        workbook.SaveAs(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while exporting class details. Error: {ex}", ex);
                throw;
            }
        }
        private static string TranslateStatus(StudentStatusType status)
        {
            switch (status)
            {
                case StudentStatusType.Active:
                    return "Đang học";
                case StudentStatusType.Suspended:
                    return "Đình chỉ";
                case StudentStatusType.Inactive:
                    return "Nghỉ học";
                default:
                    return string.Empty;
            }
        }

    }
}