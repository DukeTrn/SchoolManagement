using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Common.Enum;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Model;
using SchoolManagement.Service.Intention;

namespace SchoolManagement.Web.Controllers
{
    [ApiController, Route("api/department")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _service;
        private readonly ITeacherService _teacherService;

        public DepartmentController(IDepartmentService service, 
            ITeacherService teacherService)
        {
            _service = service;
            _teacherService = teacherService;
        }

        /// <summary>
        /// Get list of all departments
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost, Route("all")]
        public async ValueTask<IActionResult> GetAllDepartments([FromBody] PageModel queryModel)
        {
            try
            {
                var result = await _service.GetAllDepartments(queryModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        /// <summary>
        /// Get list of teachers in 1 department
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost, Route("{departmentId}/teachers/all")]
        public async ValueTask<IActionResult> GetAllTeachers(string departmentId, [FromBody] TeacherQueryModel queryModel)
        {
            try
            {
                var result = await _teacherService.GetAllTeachers(departmentId, queryModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        /// <summary>
        /// Get heads and deputies in 1 department
        /// Role: 1 (regular), 2 (deputy - phó bộ môn), 3 (head - trưởng bộ môn)
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        [HttpGet, Route("{departmentId}/teachers/heads")]
        public async Task<IActionResult> GetDepartmentHeadsAndDeputies(string departmentId)
        {
            try
            {
                var headsAndDeputies = await _teacherService.GetDepartmentHeadsAndDeputies(departmentId);
                if (headsAndDeputies == null)
                {
                    return NotFound($"Department with ID {departmentId} not found.");
                }
                //return Ok(headsAndDeputies);
                return Ok(new
                {
                    result = true,
                    data = headsAndDeputies,
                    messageType = 0
                });
            }
            catch (NotFoundException)
            {
                // Xử lý ngoại lệ và trả về lỗi nếu có
                return NotFound(new { result = false, messageType = MessageType.Error, message = $"Không tìm thấy tổ bộ môn với ID {departmentId} này!" });
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        [HttpGet, Route("filter")]
        public async Task<IActionResult> GetDepartmentFilter()
        {
            try
            {
                var depts = await _service.DepartmentFilter();
                return Ok(new
                {
                    result = true,
                    data = depts,
                    messageType = 0
                });
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        /// <summary>
        /// Create a new department
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("create")]
        public async ValueTask<IActionResult> CreateDepartment([FromBody] DepartmentAddModel model)
        {
            try
            {
                await _service.CreateDepartment(model);
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
        /// Update a department by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async ValueTask<ActionResult> UpdateDepartment(string id, [FromBody] DepartmentUpdateModel model)
        {
            try
            {
                await _service.UpdateDepartment(id, model);
                return Ok(new
                {
                    result = true,
                    messageType = MessageType.Information
                });
            }
            catch (NotFoundException)
            {
                return NotFound(new { result = false, messageType = MessageType.Error, message = $"Không tìm thấy bộ môn với ID {id} này!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(string id)
        {
            try
            {
                await _service.DeleteDepartment(id);
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
                    message = $"Không tìm thấy bộ môn với ID {id} này!"
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

        /// <summary>
        /// Add teachers (by id) into 1 department
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("add-teachers")]
        public async Task<IActionResult> AddTeachersToDepartment([FromBody] UpdateTeachersToDeptModel model)
        {
            try
            {
                await _service.AddTeachersToDepartment(model);
                return Ok(new { result = true, messageType = MessageType.Information, message = "Thêm thành công giáo viên vào bộ môn!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { result = false, messageType = MessageType.Error, message = ex.Message });
            }
        }

        /// <summary>
        /// Remove teachers (by id) from 1 department
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("remove-teachers")]
        public async Task<IActionResult> RemoveTeachersFromDepartment(UpdateTeachersToDeptModel model)
        {
            try
            {
                await _service.RemoveTeachersFromDepartment(model);
                return Ok(new { result = true, messageType = MessageType.Information, message = "Xóa thành công giáo viên khỏi bộ môn!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { result = false, messageType = MessageType.Error, message = ex.Message });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("promote-teachers")]
        public async Task<IActionResult> PromoteTeachers(PromoteTeacherModel model)
        {
            try
            {
                await _service.PromoteTeachersAsync(model);
                return Ok(new { result = true, messageType = MessageType.Information, message = "Cập nhật chức vụ thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { result = false, messageType = MessageType.Error, message = ex.Message });
            }
        }

        /// <summary>
        /// Filter (of teachers) to add teachers into department
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("teacher-filter")]
        public async Task<IActionResult> GetFilterTeachersNotInAnyDepartment()
        {
            try
            {
                var teachers = await _service.GetFilterTeachersNotInAnyDepartment();
                return Ok(new
                {
                    result = true,
                    data = teachers,
                    messageType = 0
                });
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex.Message });
            }
        }
    }
}
