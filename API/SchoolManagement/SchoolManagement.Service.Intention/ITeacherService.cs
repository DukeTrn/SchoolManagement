using SchoolManagement.Model;

namespace SchoolManagement.Service.Intention
{
    public interface ITeacherService
    {
        ValueTask<PaginationModel<TeacherDisplayModel>> GetAllTeachers(TeacherQueryModel queryModel);
        ValueTask<PaginationModel<TeacherDisplayModel>> GetAllTeachers(string departmentId, TeacherQueryModel queryModel);
        ValueTask<IEnumerable<TeacherFilterModel>> GetAllTeachersFilter();
        ValueTask<TeacherFullDisplayModel> GetTeacherById(string id);
        ValueTask<TeacherFullDisplayModel> GetTeacherByAccountId(Guid id);
        ValueTask CreateTeacher(TeacherAddModel model);
        ValueTask UpdateTeacher(string id, TeacherUpdateModel model);
        ValueTask DeleteTeacher(string id);
        Task<byte[]> ExportToExcelAsync(TeacherExportQueryModel queryModel);
    }
}
