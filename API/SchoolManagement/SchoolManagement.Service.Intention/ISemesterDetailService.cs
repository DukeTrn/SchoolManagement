using SchoolManagement.Model;

namespace SchoolManagement.Service.Intention
{
    public interface ISemesterDetailService
    {
        ValueTask<PaginationModel<SemesterDetailDisplayModel>> GetAllClassesInSem(SemesterDetailQueryModel queryModel);
        ValueTask CreateSemDetail(SemesterDetailAddModel model);
        ValueTask DeleteSemesterDetail(List<Guid> ids);
        ValueTask<IEnumerable<ClassFilterModel>> GetClassesForFilter(string semesterId);
    }
}
