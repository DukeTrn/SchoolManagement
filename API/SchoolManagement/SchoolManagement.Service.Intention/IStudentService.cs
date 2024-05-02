using SchoolManagement.Entity;
using SchoolManagement.Model;
using SchoolManagement.Model.Student;

namespace SchoolManagement.Service.Intention
{
    public interface IStudentService
    {
        ValueTask<PaginationModel<StudentModel>> GetAllStudents(StudentQueryModel queryModel);
    }
}
