using SchoolManagement.Model;

namespace SchoolManagement.Service.Intention
{
    public interface ITeacherService
    {
        ValueTask<PaginationModel<TeacherDisplayModel>> GetAllTeachers(TeacherQueryModel queryModel);
        ValueTask<TeacherFullDisplayModel> GetTeacherById(string id);
        ValueTask<TeacherFullDisplayModel> GetTeacherByAccountId(Guid id);
        ValueTask CreateTeacher(TeacherAddModel model);
        ValueTask UpdateTeacher(string id, TeacherUpdateModel model);
        ValueTask DeleteTeacher(string id);
    }
}
