using SchoolManagement.Model;

namespace SchoolManagement.Service.Intention
{
    public interface IDepartmentService
    {
        ValueTask<PaginationModel<DepartmentDisplayModel>> GetAllDepartments(PageModel queryModel);
        ValueTask<IEnumerable<DepartmentFilterModel>> DepartmentFilter();
        ValueTask<string> GetNotificationInDept(string departmentId);
        ValueTask CreateDepartment(DepartmentAddModel model);
        ValueTask UpdateDepartment(string id, DepartmentUpdateModel model);
        ValueTask DeleteDepartment(string id);
        ValueTask AddTeachersToDepartment(UpdateTeachersToDeptModel model);
        ValueTask RemoveTeachersFromDepartment(UpdateTeachersToDeptModel model);
        ValueTask PromoteTeachersAsync(PromoteTeacherModel model);
        ValueTask<IEnumerable<TeacherFilterModel>> GetFilterTeachersNotInAnyDepartment();
        ValueTask<IEnumerable<TeacherFilterModel>> GetTeachersInDeptFilter(string departmentId);
    }
}
