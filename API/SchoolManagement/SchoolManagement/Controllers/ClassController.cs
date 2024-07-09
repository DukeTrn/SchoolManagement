using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Common.Enum;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Model;
using SchoolManagement.Service.Intention;

namespace SchoolManagement.Web.Controllers
{
    [ApiController, Route("api/class")]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _service;
        private readonly ITimetableService _timetableService;

        public ClassController(IClassService service, 
            ITimetableService timetableService)
        {
            _service = service;
            _timetableService = timetableService;
        }

        /// <summary>
        /// Danh sách các lớp theo khối - Get all classes of 1 grade
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost, Route("all/{grade}")]
        public async ValueTask<IActionResult> GetAllClasses(int grade, [FromBody] PageQueryModel queryModel)
        {
            try
            {
                var result = await _service.GetAllClasses(grade, queryModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        /// <summary>
        /// (cổng GV) Danh sách các lớp gvcn dạy
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost, Route("{accountId}/homeroom-classes")]
        public async ValueTask<IActionResult> GetAllClassesByAccountId(Guid accountId, [FromBody] PageQueryModel queryModel)
        {
            try
            {
                var result = await _service.GetAllClassesByAccountId(accountId, queryModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        /// <summary>
        /// (cổng GV) Danh sách các lớp được phân công giảng dạy
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="semesterId"></param>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost, Route("{accountId}/{semesterId}/teaching-classes")]
        public async ValueTask<IActionResult> GetClassesAssignedForTeacher(Guid accountId, string semesterId, [FromBody] PageModel queryModel)
        {
            try
            {
                var result = await _service.GetClassesAssignedForTeacher(accountId, semesterId, queryModel);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }


        /// <summary>
        /// Filter GVCN chưa chủ nhiệm lớp nào - Filter to get available HR teacher (GVCN). 
        /// Format of academicYear: 2023 - 2024
        /// </summary>
        /// <param name="academicYear"></param>
        /// <returns></returns>
        [HttpGet, Route("teacher-filter/{academicYear}")]
        public async ValueTask<IActionResult> GetAvailableTeachersByGradeAsync(string academicYear)
        {
            try
            {
                var teachers = await _service.GetAvailableTeachersByGradeAsync(academicYear);
                return Ok(new
                {
                    result = true,
                    data = teachers,
                    messageType = MessageType.Information
                });
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        /// <summary>
        /// Use for class filter 
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="academicYear"></param>
        /// <returns></returns>
        [HttpGet, Route("class-filter/{academicYear}/{grade}")]
        public async ValueTask<IActionResult> GetClassesByGradeAndAcaYearFilter(int grade, string academicYear)
        {
            try
            {
                var classes = await _service.GetClassesByGradeFilter(grade, academicYear);
                return Ok(new
                {
                    result = true,
                    data = classes,
                    messageType = MessageType.Information
                });
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        /// <summary>
        /// Create a new class. Format of year: 2024 - 2025
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("create")]
        public async ValueTask<IActionResult> CreateClass([FromBody] ClassAddModel model)
        {
            try
            {
                await _service.CreateClass(model);
                return Ok(new { result = true, messageType = 0 });
            }
            catch (ExistRecordException)
            {
                return Ok(new { result = false, messageType = MessageType.Duplicated, message = "ID này đã tồn tại" });
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new { result = false, messageType = MessageType.Duplicated, Error = "Giáo viên này đã chủ nhiệm ở lớp khác!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        [HttpPut("{classId}")]
        public async Task<IActionResult> UpdateClass(string classId, string academicYear, [FromBody] ClassUpdateModel model)
        {
            try
            {
                await _service.UpdateClass(classId, academicYear, model);
                return Ok(new { result = true, messageType = 0 });
            }
            catch (NotFoundException)
            {
                return NotFound(new { result = false, messageType = MessageType.Error, message = $"Không tìm thấy lớp với ID {classId} này" });
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new { result = false, messageType = MessageType.Duplicated, Error = "Giáo viên này đã chủ nhiệm ở lớp khác!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"An unexpected error occurred: {ex.Message}" });
            }
        }

        [HttpDelete("{classId}")]
        public async Task<IActionResult> DeleteClass(string classId)
        {
            if (string.IsNullOrEmpty(classId))
            {
                return BadRequest(new { Message = "Class ID is required." });
            }

            try
            {
                await _service.DeleteClass(classId);
                return Ok(new { result = true, messageType = 0 });
            }
            catch (NotFoundException)
            {
                return NotFound(new { result = false, messageType = MessageType.Error, message = $"Không tìm thấy lớp với ID {classId} này" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"An unexpected error occurred: {ex.Message}" });
            }
        }

        #region Timetable
        /// <summary>
        /// Thời khóa biểu của lớp trong 1 học kỳ
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="semesterId"></param>
        /// <returns></returns>
        [HttpGet("timetable/{classId}/{semesterId}/list")]
        public async Task<ActionResult<List<TimetableDisplayModel>>> GetTimetablesByClassIdAsync(string classId, string semesterId)
        {
            try
            {
                var timetables = await _timetableService.GetTimetablesByClassIdAsync(classId, semesterId);
                return Ok(timetables);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while fetching timetables for ClassId {classId} and SemesterId {semesterId}. Please try again later.");
            }
        }

        /// <summary>
        /// (Cổng GV) Danh sách thời khóa biểu 
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet("timetable/teacher/{accountId}/list")]
        public async Task<IActionResult> GetTimetablesByTeacherAccountId(Guid accountId)
        {
            try
            {
                var timetables = await _timetableService.GetTimetablesByTeacherAccountIdAsync(accountId);
                return Ok(timetables);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Get timetable by id
        /// </summary>
        /// <param name="timetableId"></param>
        /// <returns></returns>
        [HttpGet("timetable/{timetableId}")]
        public async Task<IActionResult> GetTimetableById(Guid timetableId)
        {
            try
            {
                var timetable = await _timetableService.GetTimetableByIdAsync(timetableId);
                return Ok(new
                {
                    result = true,
                    data = timetable
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new
                {
                    result = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    result = false,
                    message = $"An error occurred while fetching the timetable. {ex.Message}"
                });
            }
        }

        /// <summary>
        /// tạo TKB
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("timetable/create")]
        public async Task<IActionResult> CreateTimetable([FromBody] TimetableCreateModel model)
        {
            try
            {
                await _timetableService.CreateTimetableAsync(model);
                return Ok(new { result = true, message = "Tạo thời khóa biểu mới thành công." });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { result = false, message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { result = false, message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { result = false, message = "An error occurred while creating the timetable." });
            }
        }

        /// <summary>
        /// cập nhật TKB
        /// </summary>
        /// <param name="timetableId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("timetable/{timetableId}")]
        public async Task<IActionResult> UpdateTimetable(Guid timetableId, [FromBody] TimetableUpdateModel model)
        {
            try
            {
                await _timetableService.UpdateTimetableAsync(timetableId, model);
                return Ok(new
                {
                    result = true,
                    message = "Cập nhật thời khóa biểu thành công."
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new
                {
                    result = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    result = false,
                    message = $"An error occurred while updating the timetable. {ex.Message}"
                });
            }
        }

        /// <summary>
        /// xóa TKB
        /// </summary>
        /// <param name="timetableId"></param>
        /// <returns></returns>
        [HttpDelete("timetable/{timetableId}")]
        public async Task<IActionResult> DeleteTimetable(Guid timetableId)
        {
            try
            {
                await _timetableService.DeleteTimetableAsync(timetableId);
                return Ok(new
                {
                    result = true,
                    message = "Xóa lịch thành công!"
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new
                {
                    result = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    result = false,
                    message = $"An error occurred while deleting the timetable. {ex.Message}"
                });
            }
        }
        #endregion
    }
}
