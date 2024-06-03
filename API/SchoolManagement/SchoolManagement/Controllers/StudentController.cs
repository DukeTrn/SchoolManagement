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
        /// Get list of all student (not full information)
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
        /// Use for filter
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("filter")]
        public async Task<IActionResult> GetAllStudentsFilter()
        {
            try
            {
                var students = await _service.GetAllStudentsFilter();
                return Ok(new
                {
                    result = true,
                    data = students,
                    messageType = 0
                });
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        #region Get a record
        /// <summary>
        /// Get student by ID (full information of 1 student)
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
        /// Get student by account ID (use for Account service, full information of 1 student)
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet("student/{accountId}")]
        public async ValueTask<ActionResult<StudentFullDetailModel>> GetStudentByAccountId(Guid accountId)
        {
            try
            {
                // Gọi service để lấy thông tin học sinh bằng ID
                var student = await _service.GetStudentByAccountId(accountId);
                if (student == null)
                {
                    return NotFound($"Student with account ID {accountId} not found.");
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
                return NotFound(new { result = false, messageType = MessageType.Error, message = $"Không tìm thấy học sinh với ID tài khoản này!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        #endregion

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
        /// Will delete
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("createdemo")]
        public async ValueTask<IActionResult> CreateDemoStudent([FromBody] StudentAddModel model)
        {
            try
            {
                await _service.CreateDemoStudent(model);
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
        /// Update student's information by his/her ID.
        /// Status: 1 (Active), 2 (Suspended), 3 (Inactive)
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
                    message = $"An error occurred while deleting student: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Status: 1 (Active), 2 (Suspended), 3 (Inactive)
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost("export")]
        public async Task<IActionResult> ExportToExcel([FromBody] StudentExportQueryModel queryModel)
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