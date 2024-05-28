using SchoolManagement.Model;

namespace SchoolManagement.Service.Intention
{
    public interface IConductService
    {
        ValueTask<IEnumerable<ConductDisplayModel>> GetListClassesInSemester(int grade, string semesterId, ConductQueryModel queryModel);
        ValueTask<Guid> CreateConduct(string studentId, string semesterId);
        ValueTask<PaginationModel<ConductFullDetailModel>> GetClassStudentsWithConducts(int grade, string semesterId, string classId, PageQueryModel queryModel);
        ValueTask UpdateConduct(Guid conductId, ConductUpdateModel model);

    }
}
