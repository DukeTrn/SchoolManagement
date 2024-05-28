using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Common.Enum;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Model;
using SchoolManagement.Service.Intention;

namespace SchoolManagement.Web.Controllers
{
    [ApiController, Route("api/conduct")]
    public class ConductController : ControllerBase
    {
        private readonly IConductService _service;

        public ConductController(IConductService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get list classes by grade and semester 
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="semesterId"></param>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost, Route("{grade}/{semesterId}/all")]
        public async ValueTask<IActionResult> GetListClassesInSemester(int grade, string semesterId, [FromBody] ConductQueryModel queryModel)
        {
            try
            {
                var result = await _service.GetListClassesInSemester(grade, semesterId, queryModel);
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
        [HttpPost, Route("{grade}/{semesterId}/{classId}/all")]
        public async ValueTask<IActionResult> GetClassStudentsWithConducts(int grade, string semesterId, string classId, [FromBody] PageQueryModel queryModel)
        {
            try
            {
                var result = await _service.GetClassStudentsWithConducts(grade, semesterId, classId, queryModel);
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

        [HttpPut("{id}")]
        public async ValueTask<ActionResult> UpdateConduct(Guid id, [FromForm] ConductUpdateModel model)
        {
            try
            {
                await _service.UpdateConduct(id, model);
                return Ok(new
                {
                    result = true,
                    messageType = MessageType.Information
                });
            }
            catch (NotFoundException)
            {
                return NotFound(new { result = false, messageType = MessageType.Error, message = $"Không tìm thấy hạnh kiểm với ID {id} này!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConduct(Guid id)
        {
            try
            {
                await _service.DeleteConduct(id);
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
                    message = $"Không tìm thấy học sinh có ID hạnh kiểm {id} này!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    result = false,
                    messageType = MessageType.Error,
                    message = $"An error occurred while deleting conduct: {ex.Message}"
                });
            }
        }

        [HttpPost, Route("{grade}/{semesterId}/{classId}/statistic")]
        public async ValueTask<IActionResult> GetConductClassStatistic(int grade, string semesterId, string classId)
        {
            try
            {
                var result = await _service.GetConductClassStatistic(grade, semesterId, classId);
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
        [HttpPost, Route("{grade}/{semesterId}/statistic")]
        public async ValueTask<IActionResult> GetConductSemesterStatistic(int grade, string semesterId)
        {
            try
            {
                var result = await _service.GetConductSemesterStatistic(grade, semesterId);
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
