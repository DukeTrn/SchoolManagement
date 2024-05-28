using SchoolManagement.Model;

namespace SchoolManagement.Service.Intention
{
    public interface IClassService
    {
        ValueTask<PaginationModel<ClassDisplayModel>> GetAllClasses(int grade, PageQueryModel queryModel);
        ValueTask<IEnumerable<ClassFilterModel>> GetClassesByGradeFilter(int grade);
        ValueTask<string> GetClassNameById(string classId);
        ValueTask CreateClass(ClassAddModel model);
        ValueTask UpdateClass(string classId, ClassUpdateModel model);
        ValueTask DeleteClass(string classId);
        ValueTask<IEnumerable<TeacherFilterModel>> GetAvailableTeachersByGradeAsync(int grade);

    }
}
