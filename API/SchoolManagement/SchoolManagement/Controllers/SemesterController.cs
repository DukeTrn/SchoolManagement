using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Common.Enum;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Model;
using SchoolManagement.Model.Semester;
using SchoolManagement.Service.Intention;

namespace SchoolManagement.Web.Controllers
{
    [ApiController, Route("api/semester")]
    public class SemesterController : ControllerBase
    {
        private readonly ISemesterService _service;

        public SemesterController(ISemesterService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get list of all semesters
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost, Route("all")]
        public async ValueTask<IActionResult> GetAllSemesters([FromBody] PageModel queryModel)
        {
            try
            {
                var result = await _service.GetAllSemesters(queryModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        /// <summary>
        /// Filter: Use to get all semesters  (filter lấy tất cả học kỳ)
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("filter/allsemesters")]
        public async Task<IActionResult> GetSemFilter()
        {
            try
            {
                var sems = await _service.GetSemFilter();
                return Ok(new
                {
                    result = true,
                    data = sems,
                    messageType = 0
                });
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        /// <summary>
        /// Filter: Use to get semesters based on academic year (filter lấy tất cả học kỳ trong 1 niên khóa)
        /// </summary>
        /// <param name="academicYear"></param>
        /// <returns></returns>
        [HttpGet, Route("filter/semesters")]
        public async Task<IActionResult> GetSemsByAcademicYearFilter(string academicYear)
        {
            try
            {
                var sems = await _service.GetSemsByAcademicYearFilter(academicYear);
                return Ok(new
                {
                    result = true,
                    data = sems,
                    messageType = 0
                });
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        /// <summary>
        /// Use for academic year filter (filter niên khóa)
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("filter/academicyears")]
        public async Task<IActionResult> GetAcademicYearFilter()
        {
            try
            {
                var years = await _service.GetAcademicYearFilter();
                return Ok(new
                {
                    result = true,
                    data = years,
                    messageType = 0
                });
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        /// <summary>
        /// Create a new semester. Format of semester id: 20242
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("create")]
        public async ValueTask<IActionResult> CreateSemester([FromBody] SemesterAddModel model)
        {
            try
            {
                await _service.CreateSemester(model);
                return Ok(new { result = true, messageType = 0 });
            }
            catch (ExistRecordException)
            {
                return Ok(new { result = false, messageType = MessageType.Duplicated, message = "ID này đã tồn tại" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        /// <summary>
        /// Update a semester by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async ValueTask<ActionResult> UpdateSemester(string id, [FromBody] SemesterUpdateModel model)
        {
            try
            {
                await _service.UpdateSemester(id, model);
                return Ok(new
                {
                    result = true,
                    messageType = MessageType.Information
                });
            }
            catch (NotFoundException)
            {
                return NotFound(new { result = false, messageType = MessageType.Error, message = $"Không tìm thấy học sinh với ID {id} này!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSemester(string id)
        {
            try
            {
                await _service.DeleteSemester(id);
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
                    message = $"Không tìm thấy học kì với ID {id} này!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    result = false,
                    messageType = MessageType.Error,
                    message = $"An error occurred while deleting student: {ex.Message}"
                });
            }
        }
    }
}
