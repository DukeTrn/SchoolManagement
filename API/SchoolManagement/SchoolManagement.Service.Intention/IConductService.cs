using SchoolManagement.Model;

namespace SchoolManagement.Service.Intention
{
    public interface IConductService
    {
        ValueTask<IEnumerable<ConductDisplayModel>> GetListClassesInSemester(int grade, string semesterId, ConductQueryModel queryModel);
    }
}
