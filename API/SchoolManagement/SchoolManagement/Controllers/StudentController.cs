using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Entity;
using SchoolManagement.Model.Student;
using SchoolManagement.Model;
using SchoolManagement.Service.Intention;

namespace SchoolManagement.Controllers
{
    [ApiController, Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentController(IStudentService service)
        {
            _service = service;
        }

        [HttpPost]
        public async ValueTask<PaginationModel<StudentModel>> GetAllStudents([FromBody] StudentQueryModel queryModel)
        {
            return await _service.GetAllStudents(queryModel);
        }
    }
}