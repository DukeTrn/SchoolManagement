using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Database;
using SchoolManagement.Entity;
using SchoolManagement.Model;
using SchoolManagement.Service.Intention;

namespace SchoolManagement.Service
{
    public class AssessmentService : IAssessmentService
    {
        private readonly ILogger<AssessmentEntity> _logger;
        private readonly SchoolManagementDbContext _context;

        public AssessmentService(ILogger<AssessmentEntity> logger, SchoolManagementDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Get list students in assessment (Currently grade and semesterId in param are redundant)
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="semesterId"></param>
        /// <param name="classId"></param>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public async ValueTask<PaginationModel<AssessmentDetailModel>> GetListStudentsInAssessment(int grade, string semesterId, string classId, PageQueryModel queryModel)
        {
            try
            {
                int pageNumber = queryModel.PageNumber != null && queryModel.PageNumber.Value > 0 ? queryModel.PageNumber.Value : 1;
                int pageSize = queryModel.PageSize != null && queryModel.PageSize.Value > 0 ? queryModel.PageSize.Value : 10;

                // Kiểm tra xem lớp có tồn tại không
                var classExists = await _context.ClassEntities.AnyAsync(c => c.ClassId == classId && c.Grade == grade);
                if (!classExists)
                {
                    throw new NotFoundException($"Class with ID {classId} in grade {grade} not found.");
                }

                // Lấy danh sách học sinh trong lớp và học kỳ cụ thể
                var studentQuery = from cd in _context.ClassDetailEntities
                                   join s in _context.StudentEntities on cd.StudentId equals s.StudentId
                                   join c in _context.ClassEntities on cd.ClassId equals c.ClassId
                                   where cd.ClassId == classId && c.Grade == grade
                                   select new AssessmentDetailModel
                                   {
                                       ClassDetailId = cd.ClassDetailId,
                                       StudentId = s.StudentId,
                                       FullName = s.FullName,
                                       ClassName = c.ClassName,
                                       Grade = c.Grade
                                   };

                var totalStudents = await studentQuery.CountAsync();

                var students = await studentQuery
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PaginationModel<AssessmentDetailModel>
                {
                    TotalCount = totalStudents,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    DataList = students
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting the list of students in assessment. Error: {ex}", ex.Message);
                throw;
            }
        }


        //public async ValueTask<IEnumerable<>>

        public async ValueTask CreateAssessments(List<AssessmentAddModel> models)
        {
            try
            {
                // Tạo mới các bài đánh giá từ danh sách models
                var assessments = models.Select(model => new AssessmentEntity
                {
                    AssessmentId = Guid.NewGuid(),
                    SemesterId = model.SemesterId,
                    ClassDetailId = model.ClassDetailId,
                    SubjectId = model.SubjectId,

                    Score = model.Score,
                    Weight = model.Weight,
                    Feedback = model.Feedback ?? "",

                    CreatedAt = DateTime.Now,
                    ModifiedAt = null
                }).ToList();

                // Thêm các bài đánh giá mới vào DbContext và lưu vào cơ sở dữ liệu
                _context.AssessmentEntities.AddRange(assessments);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating assessments. Error: {ex}", ex.Message);
                throw;
            }
        }

        public async ValueTask UpdateAssessment(Guid id, AssessmentUpdateModel model)
        {
            try
            {
                // Tìm bản ghi đánh giá cần cập nhật
                var assessment = await _context.AssessmentEntities.FindAsync(id);

                // Nếu không tìm thấy bản ghi, throw exception
                if (assessment == null)
                {
                    throw new NotFoundException($"Assessment with ID {id} not found.");
                }

                // Cập nhật thông tin từ model vào bản ghi tìm được
                assessment.Score = model.Score;
                assessment.Feedback = model.Feedback;
                assessment.ModifiedAt = DateTime.Now;

                // Cập nhật bản ghi trong DbContext và lưu vào cơ sở dữ liệu
                _context.AssessmentEntities.Update(assessment);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while updating assessment. Error: {ex}", ex.Message);
                throw;
            }
        }


        public async ValueTask DeleteAssessment(Guid id)
        {
            try
            {
                var data = await _context.AssessmentEntities.FindAsync(id);
                if (data == null)
                {
                    throw new NotFoundException($"Assessment with ID {id} not found.");
                }
                _context.AssessmentEntities.Remove(data);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while deleting assessment with ID {id}. Error: {ex}", id, ex);
                throw;
            }
        }
    }
}
