using SchoolManagement.Model;

namespace SchoolManagement.Service.Intention
{
    public interface IClassService
    {
        ValueTask<PaginationModel<ClassDisplayModel>> GetAllClasses(int grade, PageQueryModel queryModel);
        ValueTask<IEnumerable<ClassInSemesterModel>> GetListClassesInSemester(int grade, string semesterId);
        ValueTask<IEnumerable<ClassFilterModel>> GetClassesByGradeFilter(int grade, string academicYear);
        ValueTask<IEnumerable<ClassInSemesterModel>> GetListClassesInAcademicYear(int grade, string academicYear);
        ValueTask<string> GetClassNameById(string classId);
        ValueTask CreateClass(ClassAddModel model);
        ValueTask UpdateClass(string classId, ClassUpdateModel model);
        ValueTask DeleteClass(string classId);
        ValueTask<IEnumerable<TeacherFilterModel>> GetAvailableTeachersByGradeAsync(int grade, string academicYear);

    }
}
