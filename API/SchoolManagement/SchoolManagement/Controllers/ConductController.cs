﻿using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Common.Enum;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Model;
using SchoolManagement.Service;
using SchoolManagement.Service.Intention;

namespace SchoolManagement.Web.Controllers
{
    [ApiController, Route("api/conduct")]
    public class ConductController : ControllerBase
    {
        private readonly IConductService _service;
        private readonly IClassService _classService;

        public ConductController(IConductService service, 
            IClassService classService)
        {
            _service = service;
            _classService = classService;
        }

        /// <summary>
        /// Danh sách lớp - Get list classes by grade and semester 
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="semesterId"></param>
        /// <returns></returns>
        [HttpPost, Route("{grade}/{semesterId}/classes")]
        public async ValueTask<IActionResult> GetListClassesInSemester(int grade, string semesterId)
        {
            try
            {
                var result = await _classService.GetListClassesInSemester(grade, semesterId);
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

        /// <summary>
        /// Danh sách học sinh trong 1 lớp - Get list of students in a class in a semester
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="semesterId"></param>
        /// <param name="classId"></param>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost, Route("{grade}/{semesterId}/{classId}/students")]
        public async ValueTask<IActionResult> GetClassStudentsWithConducts(int grade, string semesterId, string classId, [FromBody] PageQueryModel queryModel)
        {
            try
            {
                var result = await _service.GetClassStudentsWithConducts(grade, semesterId, classId, queryModel);
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

        [HttpGet("student/{studentId}/semester/{semesterId}")]
        public async Task<IActionResult> GetConduct(string studentId, string semesterId)
        {
            try
            {
                var conduct = await _service.GetConduct(studentId, semesterId);
                return Ok(conduct);
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
        /// ConductType (Hạnh kiểm): 0 (null), 1 (tốt), 2 (khá), 3 (TB), 4 (yếu)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async ValueTask<ActionResult> UpdateConduct(Guid id, [FromBody] ConductUpdateModel model)
        {
            try
            {
                await _service.UpdateConduct(id, model);
                return Ok(new
                {
                    result = true,
                    messageType = MessageType.Information
                });
            }
            catch (NotFoundException)
            {
                return NotFound(new { result = false, messageType = MessageType.Error, message = $"Không tìm thấy hạnh kiểm với ID {id} này!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConduct(Guid id)
        {
            try
            {
                await _service.DeleteConduct(id);
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
                    message = $"Không tìm thấy học sinh có ID hạnh kiểm {id} này!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    result = false,
                    messageType = MessageType.Error,
                    message = $"An error occurred while deleting conduct: {ex.Message}"
                });
            }
        }

        [HttpPost, Route("{grade}/{semesterId}/{classId}/statistic")]
        public async ValueTask<IActionResult> GetConductClassStatistic(int grade, string semesterId, string classId)
        {
            try
            {
                var result = await _service.GetConductClassStatistic(grade, semesterId, classId);
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
        [HttpPost, Route("{grade}/{semesterId}/statistic")]
        public async ValueTask<IActionResult> GetConductSemesterStatistic(int grade, string semesterId)
        {
            try
            {
                var result = await _service.GetConductSemesterStatistic(grade, semesterId);
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
