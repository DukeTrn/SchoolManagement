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
        /// Get list students in assessment (Currently semesterId in param are redundant => done) (2nd page)
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
                var studentQuery = _context.ClassDetailEntities
                    .Include(cd => cd.Class)
                    .ThenInclude(c => c.HomeroomTeacher)
                    .Include(cd => cd.Student)
                    .Include(cd => cd.Class.SemClassIds)
                        .ThenInclude(sc => sc.Semester)
                    .Where(cd => cd.Class.Grade == grade
                              && cd.Class.ClassId == classId
                              && cd.Class.SemClassIds.Any(sc => sc.SemesterId == semesterId))
                    .Select(cd => new AssessmentDetailModel
                    {
                        ClassDetailId = cd.ClassDetailId,
                        StudentId = cd.Student.StudentId,
                        FullName = cd.Student.FullName,
                        ClassName = cd.Class.ClassName,
                        Grade = cd.Class.Grade,
                        AcademicYear = cd.Class.AcademicYear,
                    });

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
        /// Get list subjects and scores of 1 student in 1 class in 1 semester (sẽ gộp cả điểm và trung bình môn vào 1 bảng)
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
                _logger.LogInformation("Start to get list of subjects and scores.");

                // Truy vấn tất cả các môn học trong khối
                var subjects = await _context.SubjectEntities.Where(c => c.Grade == grade).ToListAsync();

                // Bắt lỗi nếu không tìm thấy ClassDetailId hoặc SemesterId
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

                // Truy vấn tất cả các đánh giá có liên quan đến grade, semesterId và classDetailId
                var assessments = await _context.AssessmentEntities
                    .Where(a => a.ClassDetail.Class.Grade == grade && a.SemesterId == semesterId && a.ClassDetailId == classDetailId)
                    .Include(a => a.Subject)
                    .Include(a => a.ClassDetail)
                    .ThenInclude(cd => cd.Class)
                    .ToListAsync();

                // Nhóm các đánh giá theo SubjectId
                var assessmentGroups = assessments
                    .GroupBy(a => a.SubjectId)
                    .ToDictionary(g => g.Key, g => g.ToList());

                // Tạo danh sách kết quả với điểm trung bình từng môn
                var subjectGroups = subjects.Select(subject =>
                {
                    var weight1Scores = assessmentGroups.ContainsKey(subject.SubjectId) ?
                        assessmentGroups[subject.SubjectId].Where(a => a.Weight == 1).Select(a => new ScoreModel
                        {
                            AssessmentId = a.AssessmentId,
                            Score = Math.Round(a.Score, 1),
                            Feedback = a.Feedback
                        }).ToList() : new List<ScoreModel>();

                    var weight2Scores = assessmentGroups.ContainsKey(subject.SubjectId) ?
                        assessmentGroups[subject.SubjectId].Where(a => a.Weight == 2).Select(a => new ScoreModel
                        {
                            AssessmentId = a.AssessmentId,
                            Score = Math.Round(a.Score, 1),
                            Feedback = a.Feedback
                        }).ToList() : new List<ScoreModel>();

                    var weight3Scores = assessmentGroups.ContainsKey(subject.SubjectId) ?
                        assessmentGroups[subject.SubjectId].Where(a => a.Weight == 3).Select(a => new ScoreModel
                        {
                            AssessmentId = a.AssessmentId,
                            Score = Math.Round(a.Score, 1),
                            Feedback = a.Feedback
                        }).ToList() : new List<ScoreModel>();

                    // Tính tổng điểm và trọng số của từng môn học
                    var totalWeight1Scores = weight1Scores.Sum(w => w.Score);
                    var totalWeight2Scores = weight2Scores.Sum(w => w.Score);
                    var totalWeight3Scores = weight3Scores.Sum(w => w.Score);

                    var totalWeight = weight1Scores.Count * 1 + weight2Scores.Count * 2 + weight3Scores.Count * 3;
                    var totalScores = totalWeight1Scores * 1 + totalWeight2Scores * 2 + totalWeight3Scores * 3;

                    // Tính điểm trung bình
                    var average = totalWeight > 0 ? Math.Round(totalScores / totalWeight, 2) : 0;

                    return new AssessmentScoreDisplayModel
                    {
                        ClassDetailId = classDetailId,
                        SubjectId = subject.SubjectId,
                        SubjectName = subject.SubjectName,
                        Weight1 = weight1Scores,
                        Weight2 = weight2Scores,
                        Weight3 = weight3Scores,
                        Average = average // Điểm trung bình của môn học
                    };
                }).ToList();

                return subjectGroups;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting the list of subjects and scores. Error: {ex}", ex.Message);
                throw;
            }
        }

        public async ValueTask<AssessmentScoreDisplayModel> GetListScoresOfSingleSubject(int subjectId, int grade, string semesterId, string classDetailId)
        {
            try
            {
                _logger.LogInformation("Fetching scores for Subject ID {SubjectId}, Grade {Grade}, Semester ID {SemesterId}, Class Detail ID {ClassDetailId}.",
                    subjectId, grade, semesterId, classDetailId);

                // Truy vấn tất cả các đánh giá liên quan đến môn học, lớp chi tiết và học kỳ
                var assessments = await _context.AssessmentEntities
                    .Include(a => a.Subject) // Bao gồm thông tin Subject để lấy tên môn học
                    .Where(a => a.SubjectId == subjectId
                                && a.SemesterId == semesterId
                                && a.ClassDetailId == classDetailId)
                    .ToListAsync();

                // Kiểm tra nếu không có đánh giá nào được tìm thấy
                if (!assessments.Any())
                {
                    _logger.LogWarning("No assessments found for Subject ID {SubjectId}, Semester ID {SemesterId}, Class Detail ID {ClassDetailId}.",
                        subjectId, semesterId, classDetailId);
                    return new AssessmentScoreDisplayModel
                    {
                        ClassDetailId = classDetailId,
                        SubjectId = subjectId,
                        SubjectName = string.Empty,
                        Weight1 = new List<ScoreModel>(),
                        Weight2 = new List<ScoreModel>(),
                        Weight3 = new List<ScoreModel>(),
                        Average = 0
                    };
                }

                // Lấy tên môn học
                var subjectName = assessments.First().Subject.SubjectName;

                // Phân loại điểm theo trọng số
                var weight1Scores = assessments
                    .Where(a => a.Weight == 1)
                    .Select(a => new ScoreModel
                    {
                        AssessmentId = a.AssessmentId,
                        Score = a.Score,
                        Feedback = a.Feedback
                    })
                    .ToList();

                var weight2Scores = assessments
                    .Where(a => a.Weight == 2)
                    .Select(a => new ScoreModel
                    {
                        AssessmentId = a.AssessmentId,
                        Score = a.Score,
                        Feedback = a.Feedback
                    })
                    .ToList();

                var weight3Scores = assessments
                    .Where(a => a.Weight == 3)
                    .Select(a => new ScoreModel
                    {
                        AssessmentId = a.AssessmentId,
                        Score = a.Score,
                        Feedback = a.Feedback
                    })
                    .ToList();

                // Tính toán điểm trung bình
                var totalScore = assessments.Sum(a => a.Score * a.Weight);
                var totalWeight = assessments.Sum(a => a.Weight);
                var averageScore = totalWeight != 0 ? Math.Round(totalScore / totalWeight, 2) : 0;

                // Tạo đối tượng kết quả
                var result = new AssessmentScoreDisplayModel
                {
                    ClassDetailId = classDetailId,
                    SubjectId = subjectId,
                    SubjectName = subjectName,
                    Weight1 = weight1Scores,
                    Weight2 = weight2Scores,
                    Weight3 = weight3Scores,
                    Average = averageScore
                };

                _logger.LogInformation("Successfully fetched scores for Subject ID {SubjectId}, Grade {Grade}, Semester ID {SemesterId}, Class Detail ID {ClassDetailId}.",
                    subjectId, grade, semesterId, classDetailId);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while fetching scores for Subject ID {SubjectId}, Grade {Grade}, Semester ID {SemesterId}, Class Detail ID {ClassDetailId}. Error: {Error}",
                    subjectId, grade, semesterId, classDetailId, ex.Message);
                throw;
            }
        }


        /// <summary>
        /// Get average score for each semester
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="semesterId"></param>
        /// <param name="classDetailId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async ValueTask<AverageScoreModel> GetAverageScoresForSemester(int grade, string semesterId, string classDetailId)
        {
            try
            {
                // Kiểm tra tính hợp lệ của classDetailId và semesterId
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

                // Truy vấn tất cả các môn học trong cùng một khối
                var subjects = await _context.SubjectEntities.Where(s => s.Grade == grade).ToListAsync();

                // Tạo một danh sách để lưu trữ điểm trung bình của từng môn
                var averageScores = new List<AverageEachSubjectModel>();

                // Duyệt qua từng môn để tính điểm trung bình
                foreach (var subject in subjects)
                {
                    // Truy vấn tất cả các đánh giá có liên quan đến môn học và classDetailId
                    var assessments = await _context.AssessmentEntities
                        .Where(a => a.SubjectId == subject.SubjectId && a.SemesterId == semesterId && a.ClassDetailId == classDetailId)
                        .ToListAsync();

                    // Tính điểm trung bình của môn học
                    decimal subjectAverage;
                    if (assessments.Any())
                    {
                        var totalScore = assessments.Sum(a => a.Score * a.Weight);
                        var totalWeight = assessments.Sum(a => a.Weight);
                        subjectAverage = totalWeight != 0 ? Math.Round(totalScore / totalWeight, 1) : 0;
                    }
                    else
                    {
                        // Nếu môn học chưa có điểm, điểm trung bình sẽ là 0
                        subjectAverage = 0;
                    }

                    averageScores.Add(new AverageEachSubjectModel
                    {
                        SubjectId = subject.SubjectId,
                        SubjectName = subject.SubjectName,
                        Average = subjectAverage
                    });
                }

                // Tính điểm trung bình của lớp
                var totalAverage = averageScores.Any() ? averageScores.Average(score => score.Average) : 0;

                // Tính toán AcademicPerform
                string academicPerform;
                if (totalAverage >= 8 &&
                    averageScores.All(s => s.Average >= 6.5M) &&
                    (averageScores.Any(s => s.SubjectName.Contains("Đại số & Giải tích") && s.Average >= 8) ||
                     averageScores.Any(s => s.SubjectName.Contains("Ngữ văn") && s.Average >= 8.0M)))
                {
                    academicPerform = "Giỏi";
                }
                else if (totalAverage >= 6.5M && totalAverage < 8 &&
                    averageScores.All(s => s.Average >= 5) &&
                    (averageScores.Any(s => s.SubjectName.Contains("Đại số & Giải tích") && s.Average >= 6.5M && s.Average < 8) ||
                     averageScores.Any(s => s.SubjectName.Contains("Ngữ văn") && s.Average >= 6.5M && s.Average < 8)))
                {
                    academicPerform = "Khá";
                }
                else if (totalAverage >= 5 && totalAverage < 6.5M &&
                    averageScores.All(s => s.Average >= 3.5M) &&
                    (averageScores.Any(s => s.SubjectName.Contains("Đại số & Giải tích") && s.Average >= 5 && s.Average < 6.5M) ||
                     averageScores.Any(s => s.SubjectName.Contains("Ngữ văn") && s.Average >= 5 && s.Average < 6.5M)))
                {
                    academicPerform = "Trung bình Khá";
                }
                else if (totalAverage >= 3.5M && totalAverage < 5 &&
                    averageScores.All(s => s.Average >= 2) &&
                    (averageScores.Any(s => s.SubjectName.Contains("Đại số & Giải tích") && s.Average >= 3.5M && s.Average < 5) ||
                     averageScores.Any(s => s.SubjectName.Contains("Ngữ văn") && s.Average >= 3.5M && s.Average < 5)))
                {
                    academicPerform = "Trung bình";
                }
                else
                {
                    academicPerform = "Yếu";
                }

                return new AverageScoreModel
                {
                    ClassDetailId = classDetailId,
                    TotalAverage = Math.Round(totalAverage, 1),
                    AcademicPerform = academicPerform,
                    Subjects = averageScores
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting the average scores. Error: {ex}", ex.Message);
                throw;
            }
        }


        public async ValueTask<AverageScoreForAcademicYearModel> GetAverageScoreForAcademicYear(int grade, string classDetailId, string academicYear)
        {
            try
            {
                // Kiểm tra tính hợp lệ của classDetailId
                var classDetailExist = await _context.ClassDetailEntities.AnyAsync(s => s.ClassDetailId == classDetailId);
                if (!classDetailExist)
                {
                    var errorMsg = $"Không tìm thấy Class Detail ID {classDetailId} này!";
                    _logger.LogWarning(errorMsg);
                    throw new NotFoundException(errorMsg);
                }

                // Truy vấn tất cả các môn học trong cùng một khối
                var subjects = await _context.SubjectEntities.Where(s => s.Grade == grade).ToListAsync();

                // Truy vấn các học kỳ theo năm học
                var semesters = await _context.SemesterEntities
                    .Where(s => s.AcademicYear == academicYear)
                    .OrderBy(s => s.TimeStart)
                    .ToListAsync();

                if (semesters.Count != 2)
                {
                    var errorMsg = $"Không tìm thấy hai học kỳ cho năm học {academicYear}!";
                    _logger.LogWarning(errorMsg);
                    throw new NotFoundException(errorMsg);
                }

                var firstSemesterId = semesters[0].SemesterId;
                var secondSemesterId = semesters[1].SemesterId;

                // Tạo danh sách để lưu trữ điểm trung bình của từng môn trong cả hai học kỳ
                var subjectAverages = new List<AverageEachSemesterModel>();

                foreach (var subject in subjects)
                {
                    // Truy vấn điểm số của từng môn trong học kỳ đầu tiên
                    var firstSemesterAssessments = await _context.AssessmentEntities
                        .Where(a => a.SubjectId == subject.SubjectId && a.SemesterId == firstSemesterId && a.ClassDetailId == classDetailId)
                        .ToListAsync();

                    var firstSemesterTotalScore = firstSemesterAssessments.Sum(a => a.Score * a.Weight);
                    var firstSemesterTotalWeight = firstSemesterAssessments.Sum(a => a.Weight);
                    var firstSemesterAverage = firstSemesterTotalWeight != 0 ? Math.Round(firstSemesterTotalScore / firstSemesterTotalWeight, 1) : 0;

                    // Truy vấn điểm số của từng môn trong học kỳ thứ hai
                    var secondSemesterAssessments = await _context.AssessmentEntities
                        .Where(a => a.SubjectId == subject.SubjectId && a.SemesterId == secondSemesterId && a.ClassDetailId == classDetailId)
                        .ToListAsync();

                    var secondSemesterTotalScore = secondSemesterAssessments.Sum(a => a.Score * a.Weight);
                    var secondSemesterTotalWeight = secondSemesterAssessments.Sum(a => a.Weight);
                    var secondSemesterAverage = secondSemesterTotalWeight != 0 ? Math.Round(secondSemesterTotalScore / secondSemesterTotalWeight, 1) : 0;

                    var average = Math.Round((firstSemesterAverage + secondSemesterAverage * 2) / 3, 1);

                    subjectAverages.Add(new AverageEachSemesterModel
                    {
                        SubjectId = subject.SubjectId,
                        SubjectName = subject.SubjectName,
                        FirstSemester = firstSemesterAverage,
                        SecondSemester = secondSemesterAverage,
                        Average = average
                    });
                }

                // Tính điểm trung bình của từng học kỳ và cả năm học
                var totalFirstAverage = subjectAverages.Any() ? Math.Round(subjectAverages.Average(s => s.FirstSemester), 1) : 0;
                var totalSecondAverage = subjectAverages.Any() ? Math.Round(subjectAverages.Average(s => s.SecondSemester), 1) : 0;
                var totalAverage = subjectAverages.Any() ? Math.Round(subjectAverages.Average(s => s.Average), 1) : 0;

                return new AverageScoreForAcademicYearModel
                {
                    ClassDetailId = classDetailId,
                    TotalFirstAverage = totalFirstAverage,
                    TotalSecondAverage = totalSecondAverage,
                    TotalAverage = totalAverage,
                    Subjects = subjectAverages
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting the average score for the academic year. Error: {ex}", ex.Message);
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

        public async ValueTask<AssessmentForClassStatisticModel> GetAssessmentsStatistic(int grade, string semesterId, string classId, int subjectId)
        {
            try
            {
                _logger.LogInformation($"Getting assessment statistics for class {classId}, grade {grade}, semester {semesterId}, subject {subjectId}.");

                // Lấy danh sách học sinh trong lớp và khối chỉ định
                var studentsInClass = await _context.ClassDetailEntities
                    .Where(cd => cd.ClassId == classId && cd.Class.Grade == grade)
                    .Select(cd => cd.ClassDetailId)
                    .ToListAsync();

                // Tính tổng số học sinh
                var totalStudentCount = studentsInClass.Count;

                if (totalStudentCount == 0)
                {
                    _logger.LogWarning($"No students found in class {classId}, grade {grade}, semester {semesterId}, subject {subjectId}.");
                    return new AssessmentForClassStatisticModel();
                }

                // Lấy danh sách điểm số của học sinh trong học kỳ chỉ định và môn học chỉ định
                var assessments = await _context.AssessmentEntities
                    .Where(a => a.SemesterId == semesterId && a.SubjectId == subjectId && a.Weight == 3 && studentsInClass.Contains(a.ClassDetailId))
                    .ToListAsync();

                // Khởi tạo các biến đếm
                decimal veryGoodCount = 0, goodCount = 0, averageCount = 0, weakCount = 0, poorCount = 0;

                // Phân loại điểm số vào các nhóm
                foreach (var assessment in assessments)
                {
                    var score = assessment.Score;

                    if (score >= 8 && score <= 10)
                    {
                        veryGoodCount++;
                    }
                    else if (score >= 6.5m && score < 8)
                    {
                        goodCount++;
                    }
                    else if (score >= 5 && score < 6.5m)
                    {
                        averageCount++;
                    }
                    else if (score >= 3.5m && score < 5)
                    {
                        weakCount++;
                    }
                    else if (score >= 0 && score < 3.5m)
                    {
                        poorCount++;
                    }
                }

                // Tính tỷ lệ phần trăm
                var veryGoodPercentage = (veryGoodCount / totalStudentCount * 100).ToString("0.00") + "%";
                var goodPercentage = (goodCount / totalStudentCount * 100).ToString("0.00") + "%";
                var averagePercentage = (averageCount / totalStudentCount * 100).ToString("0.00") + "%";
                var weakPercentage = (weakCount / totalStudentCount * 100).ToString("0.00") + "%";
                var poorPercentage = (poorCount / totalStudentCount * 100).ToString("0.00") + "%";

                // Trả về kết quả
                return new AssessmentForClassStatisticModel
                {
                    TotalStudent = totalStudentCount,
                    VeryGoodCount = veryGoodCount,
                    VeryGoodPercentage = veryGoodPercentage,
                    GoodCount = goodCount,
                    GoodPercentage = goodPercentage,
                    AverageCount = averageCount,
                    AveragePercentage = averagePercentage,
                    WeakCount = weakCount,
                    WeakPercentage = weakPercentage,
                    PoorCount = poorCount,
                    PoorPercentage = poorPercentage
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting assessment statistics. Error: {ex}", ex);
                throw;
            }
        }

        public async ValueTask<AssessmentForSemStatisticModel> GetAssessmentsStatisticForSem(int grade, string semesterId, int subjectId)
        {
            try
            {
                _logger.LogInformation($"Getting assessment statistics for grade {grade}, semester {semesterId}, subject {subjectId}.");

                // Lấy danh sách học sinh trong khối chỉ định
                var studentsInGrade = await _context.ClassDetailEntities
                    .Where(cd => cd.Class.Grade == grade && cd.Class.SemClassIds.Any(s => s.SemesterId == semesterId))
                    .Select(cd => cd.ClassDetailId)
                    .Distinct()
                    .ToListAsync();

                // Tính tổng số học sinh
                var totalStudentCount = studentsInGrade.Count;

                if (totalStudentCount == 0)
                {
                    _logger.LogWarning($"No students found in grade {grade}, semester {semesterId}, subject {subjectId}.");
                    return new AssessmentForSemStatisticModel();
                }

                // Lấy danh sách điểm số có weight = 3 của học sinh trong học kỳ và môn học chỉ định
                var assessments = await _context.AssessmentEntities
                    .Where(a => a.SemesterId == semesterId && a.SubjectId == subjectId && a.Weight == 3 && studentsInGrade.Contains(a.ClassDetailId))
                    .ToListAsync();

                // Khởi tạo các biến đếm
                decimal veryGoodCount = 0, goodCount = 0, averageCount = 0, weakCount = 0, poorCount = 0;

                // Phân loại điểm số vào các nhóm
                foreach (var assessment in assessments)
                {
                    var score = assessment.Score;

                    if (score >= 8 && score <= 10)
                    {
                        veryGoodCount++;
                    }
                    else if (score >= 6.5m && score < 8)
                    {
                        goodCount++;
                    }
                    else if (score >= 5 && score < 6.5m)
                    {
                        averageCount++;
                    }
                    else if (score >= 3.5m && score < 5)
                    {
                        weakCount++;
                    }
                    else if (score >= 0 && score < 3.5m)
                    {
                        poorCount++;
                    }
                }

                // Tính tỷ lệ phần trăm
                var veryGoodPercentage = (veryGoodCount / totalStudentCount * 100).ToString("0.00") + "%";
                var goodPercentage = (goodCount / totalStudentCount * 100).ToString("0.00") + "%";
                var averagePercentage = (averageCount / totalStudentCount * 100).ToString("0.00") + "%";
                var weakPercentage = (weakCount / totalStudentCount * 100).ToString("0.00") + "%";
                var poorPercentage = (poorCount / totalStudentCount * 100).ToString("0.00") + "%";

                // Trả về kết quả
                return new AssessmentForSemStatisticModel
                {
                    TotalStudents = totalStudentCount,
                    VeryGoodPercentage = veryGoodPercentage,
                    GoodPercentage = goodPercentage,
                    AveragePercentage = averagePercentage,
                    WeakPercentage = weakPercentage,
                    PoorPercentage = poorPercentage
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting assessment statistics. Error: {ex}", ex);
                throw;
            }
        }

    }
}
