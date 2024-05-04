using Microsoft.AspNetCore.Mvc;
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

        [HttpPost, Route("all")]
        public async ValueTask<PaginationModel<StudentDisplayModel>> GetAllStudents([FromBody] StudentQueryModel queryModel)
        {
            return await _service.GetAllStudents(queryModel);
        }

        [HttpPost, Route("create")]
        //public async ValueTask<IActionResult> CreateStudent([FromBody] StudentAddModel queryModel)
        //{
        //    await _service.CreateStudent(queryModel);
        //}
        public async ValueTask<IActionResult> CreateStudent(StudentAddModel model)
        {
            try
            {
                await _service.CreateStudent(model);
                return Ok(new { result = true, messageType = 0 });
            }
            catch (ExistRecordException ex)
            {
                // Log ex.LogMessage if needed
                // Notify user based on ex.NotifactionType
                // Handle error data in ex.ErrorData if needed

                return Ok(new { result = false, messageType = 2, message = "ID này đã tồn tại" });
            }
            catch (Exception ex)
            {
                // Log other exceptions if needed
                return StatusCode(500, new { result = false, messageType = 2 });
            }
        }
    }
}