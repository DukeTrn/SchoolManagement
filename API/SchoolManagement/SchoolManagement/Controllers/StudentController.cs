using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Entity;
using SchoolManagement.Model;
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
        public async ValueTask<bool> CreateStudent([FromBody] StudentAddModel queryModel)
        {
            return await _service.CreateStudent(queryModel);
        }
    }
}