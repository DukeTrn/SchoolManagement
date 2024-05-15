using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Common.Enum;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Model;
using SchoolManagement.Service.Intention;
using SchoolManagement.Service.Intention.Data;

namespace SchoolManagement.Controllers
{
    [ApiController, Route("api/student")]
    //[Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;
        private readonly ICloudinaryService _cloudinary;

        public StudentController(IStudentService service, ICloudinaryService cloudinary)
        {
            _service = service;
            _cloudinary = cloudinary;
        }

        /// <summary>
        /// Get list of all student
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost, Route("all")]
        public async ValueTask<IActionResult> GetAllStudents([FromBody] StudentQueryModel queryModel)
        {
            try
            {
                var result = await _service.GetAllStudents(queryModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
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
        public async ValueTask<IActionResult> CreateStudent([FromForm] StudentAddModel model)
        {
            try
            {
                await _service.CreateStudent(model);
                return Ok(new { result = true, messageType = 0 });
            }
            catch (ExistRecordException)
            {
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
        public async ValueTask<ActionResult> UpdateStudent(string id, [FromForm] StudentUpdateModel model)
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

        [HttpPost("export")]
        public async Task<IActionResult> ExportToExcel([FromBody] ExportQueryModel queryModel)
        {
            try
            {
                byte[] fileContents = await _service.ExportToExcelAsync(queryModel);

                // Trả về tệp Excel đã xuất
                string fileName = $"Quản lý học sinh_{DateTime.Now:ddMMyyyyHHmmss}.xlsx";
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi xuất dữ liệu: {ex.Message}");
            }
        }

        [HttpPost("upload-avatar")]
        public async Task<IActionResult> UploadAvatar([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var uploadResult = await _cloudinary.UploadImageAsync(file);
            return Ok(new { Url = uploadResult });
        }
    }
}