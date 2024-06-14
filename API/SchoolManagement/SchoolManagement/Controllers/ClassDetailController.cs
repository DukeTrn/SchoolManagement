using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Common.Enum;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Model;
using SchoolManagement.Model.ClassDetail;
using SchoolManagement.Service.Intention;

namespace SchoolManagement.Web.Controllers
{
    [ApiController, Route("api/classdetail")]
    public class ClassDetailController : ControllerBase
    {
        private readonly IClassDetailService _service;
        private readonly IClassService _classService;

        public ClassDetailController(IClassDetailService service, 
            IClassService classService)
        {
            _service = service;
            _classService = classService;
        }

        /// <summary>
        /// Get students of 1 class (danh sách học sinh trong 1 lớp).
        /// Status: 1 (active), 2 (suspended)
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost, Route("all/{classId}")]
        public async ValueTask<IActionResult> GetAllClassDetail(string classId, [FromBody] ClassDetailQueryModel queryModel)
        {
            try
            {
                var result = await _service.GetAllClassDetail(classId, queryModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        /// <summary>
        /// Filter học sinh - Filter (for students) to add students into class. 
        /// Format of academicYear: 2023 - 2024
        /// </summary>
        /// <param name="academicYear"></param>
        /// <param name="grade"></param>
        /// <returns></returns>
        [HttpGet("filter-students")]
        public async Task<ActionResult> FilterStudentsByGrade(string academicYear, int grade)
        {
            try
            {
                var filteredStudents = await _service.FilterStudentsByGrade(academicYear, grade);
                return Ok(new
                {
                    result = true,
                    data = filteredStudents,
                    messageType = MessageType.Information
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Thêm học sinh vào lớp - Add students into a class (same classId)
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost, Route("create")]
        public async ValueTask<IActionResult> Create(List<ClassDetailAddModel> models)
        {
            try
            {
                await _service.AddClassDetails(models);
                return Ok(new { result = true, messageType = 0 });
            }
            catch (NotFoundException)
            {
                return Ok(new { result = false, messageType = MessageType.Error, message = "Không tìm thấy id lớp hoặc id học sinh" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { result = false, messageType = MessageType.Error, message = ex.Message });
            }
        }

        /// <summary>
        /// Update class Id of student to move into another class (chuyển lớp)
        /// </summary>
        /// <param name="classDetailId"></param>
        /// <param name="newClassId"></param>
        /// <returns></returns>
        [HttpPut("{classDetailId}/{newClassId}")]
        public async Task<IActionResult> UpdateClassDetailAsync(string classDetailId, string newClassId)
        {
            try
            {
                await _service.UpdateClassDetailAsync(classDetailId, newClassId);
                return Ok(new { result = true, messageType = 0 });
            }
            catch (NotFoundException)
            {
                return NotFound(new { result = false, messageType = MessageType.Error, message = $"Không tìm thấy ClassDetailId hoặc ClassID này" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"An unexpected error occurred: {ex.Message}" });
            }
        }

        [HttpDelete("{classDetailId}")]
        public async Task<IActionResult> DeleteClassDetail(string classDetailId)
        {
            if (string.IsNullOrEmpty(classDetailId))
            {
                return BadRequest(new { Message = "ClassDetail ID is required." });
            }

            try
            {
                await _service.DeleteClassDetail(classDetailId);
                return Ok(new { result = true, messageType = 0 });
            }
            catch (NotFoundException)
            {
                return NotFound(new { result = false, messageType = MessageType.Error, message = $"Không tìm thấy ID {classDetailId} này" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"An unexpected error occurred: {ex.Message}" });
            }
        }

        [HttpPost("export")]
        public async Task<IActionResult> ExportToExcel(string classId, [FromBody]ClassDetailExportQueryModel queryModel)
        {
            try
            {
                byte[] fileContents = await _service.ExportClassDetailToExcelAsync(classId, queryModel);

                var className = await _classService.GetClassNameById(classId);

                // Trả về tệp Excel đã xuất
                string fileName = $"Danh sách học sinh_{className}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx";
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi xuất dữ liệu: {ex.Message}");
            }
        }
    }
}
