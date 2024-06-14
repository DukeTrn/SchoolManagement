using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Common.Enum;
using SchoolManagement.Model;
using SchoolManagement.Service;
using SchoolManagement.Service.Intention;

namespace SchoolManagement.Web.Controllers
{
    [ApiController, Route("api/assignment")]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _service;

        public AssignmentController(IAssignmentService service)
        {
            _service = service;
        }

        /// <summary>
        ///  Danh sách Phân công giáo viên - Use this api after clicking view detail of subject
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="semesterId"></param>
        /// <param name="subjectId"></param>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost("all/{grade}/{subjectId}/{semesterId}")]
        public async Task<IActionResult> GetListAssignments(int grade, string semesterId, int subjectId, [FromBody] AssignmentQueryModel queryModel)
        {
            try
            {
                var assignments = await _service.GetListAssignments(grade, semesterId, subjectId, queryModel);
                return Ok(new
                {
                    result = true,
                    data = assignments,
                    messageType = MessageType.Information
                });
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("create")]
        public async Task<IActionResult> CreateAssignment(AssignmentAddModel model)
        {
            try
            {
                await _service.CreateAssignment(model);
                return Ok(new { result = true, messageType = 0 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Update assignment (chuyển lớp cho giáo viên)
        /// </summary>
        /// <param name="assignmentId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        [HttpPut, Route("update/{assignmentId}")]
        public async Task<IActionResult> UpdateAssignment(Guid assignmentId, string classId)
        {
            try
            {
                await _service.UpdateAssignment(assignmentId, classId);
                return Ok(new { result = true, messageType = 0 });
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Không tìm thấy mã phân công giáo viên này!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{assignmentId}")]
        public async Task<IActionResult> DeleteAssignment(Guid assignmentId)
        {
            try
            {
                await _service.DeleteAssignment(assignmentId);
                return Ok(new { result = true, messageType = 0 });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound("Không tìm thấy mã phân công giáo viên này!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
