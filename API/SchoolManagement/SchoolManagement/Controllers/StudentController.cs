using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Common.Enum;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Entity;
using SchoolManagement.Model;
using SchoolManagement.Service;
using SchoolManagement.Service.Intention;

namespace SchoolManagement.Controllers
{
    [ApiController, Route("api/student")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentController(IStudentService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get list of all student
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost, Route("all")]
        public async ValueTask<PaginationModel<StudentDisplayModel>> GetAllStudents([FromBody] StudentQueryModel queryModel)
        {
            return await _service.GetAllStudents(queryModel);
        }

        /// <summary>
        /// Get student by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async ValueTask<ActionResult<StudentFullDetailModel>> GetStudentById(string id)
        {
            try
            {
                // Gọi service để lấy thông tin học sinh bằng ID
                var student = await _service.GetStudentById(id);
                if (student == null)
                {
                    return NotFound($"Student with ID {id} not found.");
                }

                // Trả về thông tin học sinh nếu tìm thấy
                return Ok(new
                {
                    result = true,
                    data = student,
                    messageType = 0
                });
            }
            catch (NotFoundException)
            {
                // Xử lý ngoại lệ và trả về lỗi nếu có
                return NotFound(new { result = false, messageType = MessageType.Error, message = $"Không tìm thấy học sinh với ID {id} này!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Create a new student
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("create")]
        public async ValueTask<IActionResult> CreateStudent([FromBody] StudentAddModel model)
        {
            try
            {
                await _service.CreateStudent(model);
                return Ok(new { result = true, messageType = 0 });
            }
            catch (ExistRecordException)
            {
                // Log ex.LogMessage if needed
                // Notify user based on ex.NotifactionType
                // Handle error data in ex.ErrorData if needed

                return Ok(new { result = false, messageType = MessageType.Duplicated, message = "ID này đã tồn tại" });
            }
            catch (Exception ex)
            {
                // Log other exceptions if needed
                return StatusCode(500, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        /// <summary>
        /// Update student's information by his/her ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async ValueTask<ActionResult> UpdateStudent(string id, [FromBody] StudentUpdateModel model)
        {
            try
            {
                await _service.UpdateStudent(id, model);
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
        public async Task<IActionResult> DeleteStudent(string id)
        {
            try
            {
                await _service.DeleteStudent(id);
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
                    message = $"Không tìm thấy học sinh với ID {id} này!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    result = false,
                    messageType = MessageType.Error,
                    message = $"An error occurred while deleting student: {ex}"
                });
            }
        }
    }
}