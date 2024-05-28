using SchoolManagement.Model;
using SchoolManagement.Model.Semester;

namespace SchoolManagement.Service.Intention
{
    public interface ISemesterService
    {
        ValueTask<PaginationModel<SemesterDisplayModel>> GetAllSemesters(PageModel queryModel);
        ValueTask CreateSemester(SemesterAddModel model);
        ValueTask UpdateSemester(string id, SemesterUpdateModel model);
        ValueTask DeleteSemester(string id);
        ValueTask<IEnumerable<SemesterFilterModel>> GetFilter();
    }
}
