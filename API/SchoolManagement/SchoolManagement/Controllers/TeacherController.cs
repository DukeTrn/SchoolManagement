﻿using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Common.Enum;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Model;
using SchoolManagement.Service.Intention;
using SchoolManagement.Service.Intention.Data;

namespace SchoolManagement.Web.Controllers
{
    [ApiController, Route("api/teacher")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _service;
        private readonly ICloudinaryService _cloudinary;

        public TeacherController(ITeacherService service, ICloudinaryService cloudinary)
        {
            _service = service;
            _cloudinary = cloudinary;
        }

        /// <summary>
        /// Get list of all teachers (not full information)
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost, Route("all")]
        public async ValueTask<IActionResult> GetAllStudents([FromBody] TeacherQueryModel queryModel)
        {
            try
            {
                var result = await _service.GetAllTeachers(queryModel);
                return Ok(result);
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
        public async ValueTask<ActionResult<TeacherFullDisplayModel>> GetTeacherById(string id)
        {
            try
            {
                var teacher = await _service.GetTeacherById(id);
                if (teacher == null)
                {
                    return NotFound($"Teacher with ID {id} not found.");
                }

                return Ok(new
                {
                    result = true,
                    data = teacher,
                    messageType = 0
                });
            }
            catch (NotFoundException)
            {
                // Xử lý ngoại lệ và trả về lỗi nếu có
                return NotFound(new { result = false, messageType = MessageType.Error, message = $"Không tìm thấy giáo viên với ID {id} này!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Get teacher by account ID (use for Account service, full information of 1 teacher)
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet("teacher/{accountId}")]
        public async ValueTask<ActionResult<StudentFullDetailModel>> GetTeacherByAccountId(Guid accountId)
        {
            try
            {
                var teacher = await _service.GetTeacherByAccountId(accountId);
                if (teacher == null)
                {
                    return NotFound($"Teacher with account ID {accountId} not found.");
                }
                return Ok(new
                {
                    result = true,
                    data = teacher,
                    messageType = 0
                });
            }
            catch (NotFoundException)
            {
                return NotFound(new { result = false, messageType = MessageType.Error, message = $"Không tìm thấy giáo viên với ID tài khoản này!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        #endregion

        /// <summary>
        /// Create a new teacher
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("create")]
        public async ValueTask<IActionResult> CreateTeacher([FromForm] TeacherAddModel model)
        {
            try
            {
                await _service.CreateTeacher(model);
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
        /// Update teacher's information by his/her ID.
        /// Status: 1 (Active), 2 (Suspended), 3 (Inactive)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async ValueTask<ActionResult> UpdateTeacher(string id, [FromForm] TeacherUpdateModel model)
        {
            try
            {
                await _service.UpdateTeacher(id, model);
                return Ok(new
                {
                    result = true,
                    messageType = MessageType.Information
                });
            }
            catch (NotFoundException)
            {
                return NotFound(new { result = false, messageType = MessageType.Error, message = $"Không tìm thấy giáo viên với ID {id} này!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(string id)
        {
            try
            {
                await _service.DeleteTeacher(id);
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
                    message = $"Không tìm thấy giáo viên với ID {id} này!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    result = false,
                    messageType = MessageType.Error,
                    message = $"An error occurred while deleting giáo viên: {ex.Message}"
                });
            }
        }
    }
}
