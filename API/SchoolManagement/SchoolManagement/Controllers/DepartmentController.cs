using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Common.Enum;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Model;
using SchoolManagement.Service;
using SchoolManagement.Service.Intention;

namespace SchoolManagement.Web.Controllers
{
    [ApiController, Route("api/department")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _service;

        public DepartmentController(IDepartmentService service)
        {
            _service = service;
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
        /// Role: 1 (regular), 2 (deputy head), 3 (head)
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
    }
}
