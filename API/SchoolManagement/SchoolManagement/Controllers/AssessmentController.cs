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
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        [HttpPost, Route("create")]
        public async ValueTask<IActionResult> CreateAssessments([FromForm] List<AssessmentAddModel> models)
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

        [HttpPut("{id}")]
        public async ValueTask<ActionResult> UpdateConduct(Guid id, [FromForm] AssessmentUpdateModel model)
        {
            try
            {
                await _assessmentService.UpdateAssessment(id, model);
                return Ok(new
                {
                    result = true,
                    messageType = MessageType.Information
                });
            }
            catch (NotFoundException)
            {
                return NotFound(new { result = false, messageType = MessageType.Error, message = $"Không tìm thấy bài kiểm tra với ID {id} này!" });
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
    }
}
