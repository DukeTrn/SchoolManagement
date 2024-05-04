using SchoolManagement.Entity;
using SchoolManagement.Model;

namespace SchoolManagement.Service.Intention
{
    public interface IStudentService
    {
        ValueTask<PaginationModel<StudentDisplayModel>> GetAllStudents(StudentQueryModel queryModel);
        ValueTask CreateStudent(StudentAddModel model);
        ValueTask<StudentFullDetailModel> GetStudentById(string studentId);
    }
}
