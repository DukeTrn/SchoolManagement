using SchoolManagement.Entity;
using SchoolManagement.Model;

namespace SchoolManagement.Service.Intention
{
    public interface IStudentService
    {
        ValueTask<PaginationModel<StudentDisplayModel>> GetAllStudents(StudentQueryModel queryModel);
        ValueTask<bool> CreateStudent(StudentAddModel model);

    }
}
