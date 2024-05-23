using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Common.Enum;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Model.ClassDetail;
using SchoolManagement.Service;
using SchoolManagement.Service.Intention;

namespace SchoolManagement.Web.Controllers
{
    [ApiController, Route("api/classdetail")]
    public class ClassDetailController : ControllerBase
    {
        private readonly IClassDetailService _service;

        public ClassDetailController(IClassDetailService service)
        {
            _service = service;
        }


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
        //[HttpPost("add-multiple")]
        //public async Task<IActionResult> AddClassDetails(List<ClassDetailAddModel> models)
        //{
        //    try
        //    {
        //        if (models == null || models.Count == 0)
        //        {
        //            return BadRequest("No class details provided.");
        //        }

        //        var result = await _service.AddClassDetails(models);

        //        if (result)
        //        {
        //            return Ok("Class details added successfully.");
        //        }
        //        else
        //        {
        //            return StatusCode(500, "Failed to add class details.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "An error occurred while processing your request.");
        //    }
        //}
    }
}
