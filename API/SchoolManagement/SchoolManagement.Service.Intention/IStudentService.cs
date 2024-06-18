using SchoolManagement.Entity;
using SchoolManagement.Model;
using SchoolManagement.Model.Student;
using System.Data;

namespace SchoolManagement.Service.Intention
{
    public interface IStudentService
    {
        ValueTask<PaginationModel<StudentDisplayModel>> GetAllStudents(StudentQueryModel queryModel);
        ValueTask<IEnumerable<StudentFilterModel>> GetAllStudentsFilter();
        ValueTask<StudentFullDetailModel> GetStudentById(string studentId);
        ValueTask<StudentFullDetailModel> GetStudentByAccountId(Guid accountId);
        ValueTask CreateStudent(StudentAddModel model);
        ValueTask UpdateStudent(string id, StudentUpdateModel model);
        ValueTask DeleteStudent(string id);
        Task<byte[]> ExportToExcelAsync(StudentExportQueryModel queryModel);
        ValueTask<IEnumerable<StudentInClassModel>> GetStudentInClasses(Guid accountId);

        // will delete
        ValueTask CreateDemoStudent(StudentAddModel model);
    }
}
