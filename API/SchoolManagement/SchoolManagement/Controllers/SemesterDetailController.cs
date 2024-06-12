using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Common.Enum;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Model;
using SchoolManagement.Model.ClassDetail;
using SchoolManagement.Service.Intention;

namespace SchoolManagement.Web.Controllers
{
    [ApiController, Route("api/semesterdetail")]
    public class SemesterDetailController : ControllerBase
    {
        private readonly ISemesterDetailService _service;
        
        public SemesterDetailController(ISemesterDetailService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all classes in 1 semester
        /// </summary>
        /// <param name="semesterId"></param>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost, Route("all")]
        public async ValueTask<IActionResult> GetAllClassDetail(string semesterId, [FromBody] SemesterDetailQueryModel queryModel)
        {
            try
            {
                var result = await _service.GetAllClassesInSem(semesterId, queryModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        /// <summary>
        /// Filter for add classes into 1 semester
        /// </summary>
        /// <param name="semesterId"></param>
        /// <returns></returns>
        [HttpGet("filter/{semesterId}")]
        public async Task<ActionResult> FilterStudentsByGrade(string semesterId)
        {
            try
            {
                var filterClasses = await _service.GetClassesForFilter(semesterId);
                return Ok(new
                {
                    result = true,
                    data = filterClasses,
                    messageType = MessageType.Information
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Add classes into 1 semester
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost, Route("create")]
        public async ValueTask<IActionResult> Create(SemesterDetailAddModel models)
        {
            try
            {
                await _service.CreateSemDetail(models);
                return Ok(new { result = true, messageType = 0 });
            }
            catch (KeyNotFoundException)
            {
                return Ok(new { result = false, messageType = MessageType.Error, message = "Không tìm thấy id lớp hoặc id học kì" });
            }
            catch (InvalidOperationException ex)
            {
                return Ok(new { result = false, messageType = MessageType.Error, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { result = false, messageType = MessageType.Error, message = ex.Message });
            }
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> DeleteClassDetail([FromBody]List<Guid> ids)
        {
            try
            {
                await _service.DeleteSemesterDetail(ids);
                return Ok(new { result = true, messageType = 0 });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { result = false, messageType = MessageType.Error, message = $"Không tìm thấy ID {ids} này" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"An unexpected error occurred: {ex.Message}" });
            }
        }
    }
}
