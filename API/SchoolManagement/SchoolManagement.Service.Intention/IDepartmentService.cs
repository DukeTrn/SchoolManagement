using SchoolManagement.Entity;
using SchoolManagement.Model;
using SchoolManagement.Model.Department;

namespace SchoolManagement.Service.Intention
{
    public interface IDepartmentService
    {
        ValueTask<PaginationModel<DepartmentDisplayModel>> GetAllDepartments(PageModel queryModel);
        ValueTask CreateDepartment(DepartmentAddModel model);
        ValueTask UpdateDepartment(string id, DepartmentUpdateModel model);
        ValueTask DeleteDepartment(string id);
    }
}
