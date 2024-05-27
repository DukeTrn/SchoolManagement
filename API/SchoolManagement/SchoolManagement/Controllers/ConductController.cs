using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Common.Enum;
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
        [HttpPost, Route("all/{grade}/{semesterId}")]
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
    }
}
