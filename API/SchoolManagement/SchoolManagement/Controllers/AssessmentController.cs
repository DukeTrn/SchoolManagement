using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Common.Enum;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Model;
using SchoolManagement.Service.Intention;

namespace SchoolManagement.Web.Controllers
{
    [ApiController, Route("api/assessment")]
    public class AssessmentController : ControllerBase
    {
        private readonly IAssessmentService _assessmentService;
        private readonly IClassService _classService;

        public AssessmentController(IAssessmentService assessmentService, 
            IClassService classService)
        {
            _assessmentService = assessmentService;
            _classService = classService;
        }

        /// <summary>
        /// Get list classes by grade and semester 
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="semesterId"></param>
        /// <returns></returns>
        [HttpPost, Route("{grade}/{semesterId}/classes")]
        public async ValueTask<IActionResult> GetListClassesInSemester(int grade, string semesterId)
        {
            try
            {
                var result = await _classService.GetListClassesInSemester(grade, semesterId);
                return Ok(new
                {
                    result = true,
                    data = result,
                    messageType = 0
                });
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        /// <summary>
        /// Get list of students in a class in a semester
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="semesterId"></param>
        /// <param name="classId"></param>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost, Route("{grade}/{semesterId}/{classId}/students")]
        public async ValueTask<IActionResult> GetClassStudentsWithConducts(int grade, string semesterId, string classId, [FromBody] PageQueryModel queryModel)
        {
            try
            {
                var result = await _assessmentService.GetListStudentsInAssessment(grade, semesterId, classId, queryModel);
                return Ok(new
                {
                    result = true,
                    data = result,
                    messageType = 0
                });
            }
            catch (NotFoundException)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = $"Không tìm thấy mã lớp học {classId} hoặc khối {grade}" });
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        /// <summary>
        /// List subjects and scores of 1 student
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="semesterId"></param>
        /// <param name="classDetailId"></param>
        /// <returns></returns>
        [HttpPost, Route("{grade}/{semesterId}/{classDetailId}/scores")]
        public async ValueTask<IActionResult> GetListSubjectAndScore(int grade, string semesterId, string classDetailId)
        {
            try
            {
                var result = await _assessmentService.GetListSubjectAndScore(grade, semesterId, classDetailId);
                return Ok(new
                {
                    result = true,
                    data = result,
                    messageType = 0
                });
            }
            catch (NotFoundException)
            {
                return NotFound(new
                {
                    result = false,
                    messageType = MessageType.Error,
                    message = $"Không tìm thấy học kì {semesterId} hoặc id {classDetailId} này!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        /// <summary>
        /// Average score for each semester of 1 student(trung bình điểm các môn cho từng học kì)
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="semesterId"></param>
        /// <param name="classDetailId"></param>
        /// <returns></returns>
        [HttpPost, Route("{grade}/{semesterId}/{classDetailId}/semester-average")]
        public async ValueTask<IActionResult> GetAverageScoresForSemester(int grade, string semesterId, string classDetailId)
        {
            try
            {
                var result = await _assessmentService.GetAverageScoresForSemester(grade, semesterId, classDetailId);
                return Ok(new
                {
                    result = true,
                    data = result,
                    messageType = 0
                });
            }
            catch (NotFoundException)
            {
                return NotFound(new
                {
                    result = false,
                    messageType = MessageType.Error,
                    message = $"Không tìm thấy học kì {semesterId} hoặc id {classDetailId} này!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        /// <summary>
        /// Average score for a year of 1 student(trung bình điểm các môn và điểm trung bình của cả năm học)
        /// Format of academicYear: 2023 - 2024
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="classDetailId"></param>
        /// <param name="academicYear"></param>
        /// <returns></returns>
        [HttpPost, Route("{grade}/{academicYear}/{classDetailId}/year-average")]
        public async ValueTask<IActionResult> GetAverageScoreForAcademicYear(int grade, string classDetailId, string academicYear)
        {
            try
            {
                var result = await _assessmentService.GetAverageScoreForAcademicYear(grade, classDetailId, academicYear);
                return Ok(new
                {
                    result = true,
                    data = result,
                    messageType = 0
                });
            }
            catch (NotFoundException)
            {
                return NotFound(new
                {
                    result = false,
                    messageType = MessageType.Error,
                    message = $"Không tìm thấy niên khóa {academicYear} hoặc id {classDetailId} này!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        [HttpPost, Route("create")]
        public async ValueTask<IActionResult> CreateAssessments([FromBody] List<AssessmentAddModel> models)
        {
            try
            {
                await _assessmentService.CreateAssessments(models);
                return Ok(new { result = true, messageType = 0 });
            }
            catch (Exception ex)
            {
                // Log other exceptions if needed
                return StatusCode(500, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        [HttpPut("{assessmentId}")]
        public async ValueTask<ActionResult> UpdateConduct(Guid assessmentId, [FromBody] AssessmentUpdateModel model)
        {
            try
            {
                await _assessmentService.UpdateAssessment(assessmentId, model);
                return Ok(new
                {
                    result = true,
                    messageType = MessageType.Information
                });
            }
            catch (NotFoundException)
            {
                return NotFound(new { result = false, messageType = MessageType.Error, message = $"Không tìm thấy bài kiểm tra với ID {assessmentId} này!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssessment(Guid id)
        {
            try
            {
                await _assessmentService.DeleteAssessment(id);
                return Ok(new
                {
                    result = true,
                    messageType = MessageType.Information
                });
            }
            catch (NotFoundException)
            {
                return NotFound(new
                {
                    result = false,
                    messageType = MessageType.Error,
                    message = $"Không tìm thấy bài kiểm tra có ID {id} này!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    result = false,
                    messageType = MessageType.Error,
                    message = $"An error occurred while deleting assessment: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Thống kê điểm cuối kì theo môn học (1 lớp trong 1 học kì)
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="semesterId"></param>
        /// <param name="classId"></param>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        [HttpPost, Route("{grade}/{semesterId}/{classId}/{subjectId}/class-statistic")]
        public async ValueTask<IActionResult> GetAssessmentsStatistic(int grade, string semesterId, string classId, int subjectId)
        {
            try
            {
                var result = await _assessmentService.GetAssessmentsStatistic(grade, semesterId, classId, subjectId);
                return Ok(new
                {
                    result = true,
                    data = result,
                    messageType = 0
                });
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        /// <summary>
        /// Thống kê điểm cuối kì theo môn học (tất cả các lớp trong 1 học kì)
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="semesterId"></param>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        [HttpPost, Route("{grade}/{semesterId}/{subjectId}/sem-statistic")]
        public async ValueTask<IActionResult> GetAssessmentsStatisticForSem(int grade, string semesterId, int subjectId)
        {
            try
            {
                var result = await _assessmentService.GetAssessmentsStatisticForSem(grade, semesterId, subjectId);
                return Ok(new
                {
                    result = true,
                    data = result,
                    messageType = 0
                });
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }
    }
}
