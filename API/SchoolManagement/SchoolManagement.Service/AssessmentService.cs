using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Database;
using SchoolManagement.Entity;
using SchoolManagement.Model;
using SchoolManagement.Model.Assessment;
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

        /// <summary>
        /// Get list subjects and scores of 1 student in 1 class in 1 semester
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="semesterId"></param>
        /// <param name="classDetailId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async ValueTask<IEnumerable<AssessmentScoreDisplayModel>> GetListSubjectAndScore(int grade, string semesterId, string classDetailId)
        {
            try
            {
                // Truy vấn tất cả các môn học trong khối
                var subjects = await _context.SubjectEntities.Where(c => c.Grade == grade).ToListAsync();

                // Bắt lỗi nếu không tìm thấy
                var classDetailExist = await _context.ClassDetailEntities.AnyAsync(s => s.ClassDetailId == classDetailId);
                if (!classDetailExist)
                {
                    //return Enumerable.Empty<AssessmentScoreDisplayModel>();
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


                // Truy vấn tất cả các đánh giá có liên quan đến grade và semesterId
                var assessments = await _context.AssessmentEntities
                    .Where(a => a.ClassDetail.Class.Grade == grade && a.SemesterId == semesterId)
                    .Include(a => a.Subject)
                    .Include(a => a.ClassDetail)
                    .ThenInclude(cd => cd.Class)
                    .ToListAsync();

                // Nhóm các đánh giá theo SubjectId
                var assessmentGroups = assessments
                    .GroupBy(a => a.SubjectId)
                    .ToDictionary(g => g.Key, g => g.ToList());

                // Tạo danh sách kết quả
                var subjectGroups = subjects.Select(subject => new AssessmentScoreDisplayModel
                {
                    ClassDetailId = classDetailId,
                    SubjectId = subject.SubjectId,
                    SubjectName = subject.SubjectName,
                    Weight1 = assessmentGroups.ContainsKey(subject.SubjectId) ?
                        assessmentGroups[subject.SubjectId].Where(a => a.Weight == 1).Select(a => new ScoreModel
                        {
                            AssessmentId = a.AssessmentId,
                            Score = Math.Round(a.Score,1),
                            Feedback = a.Feedback
                        }).ToList() : new List<ScoreModel>(),
                    Weight2 = assessmentGroups.ContainsKey(subject.SubjectId) ?
                        assessmentGroups[subject.SubjectId].Where(a => a.Weight == 2).Select(a => new ScoreModel
                        {
                            AssessmentId = a.AssessmentId,
                            Score = Math.Round(a.Score, 1),
                            Feedback = a.Feedback
                        }).ToList() : new List<ScoreModel>(),
                    Weight3 = assessmentGroups.ContainsKey(subject.SubjectId) ?
                        assessmentGroups[subject.SubjectId].Where(a => a.Weight == 3).Select(a => new ScoreModel
                        {
                            AssessmentId = a.AssessmentId,
                            Score = Math.Round(a.Score, 1),
                            Feedback = a.Feedback
                        }).ToList() : new List<ScoreModel>(),
                }).ToList();

                return subjectGroups;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting the list of subjects and scores. Error: {ex}", ex.Message);
                throw;
            }
        }


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
