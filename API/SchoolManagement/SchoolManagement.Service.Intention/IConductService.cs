using SchoolManagement.Model;

namespace SchoolManagement.Service.Intention
{
    public interface IConductService
    {
        ValueTask<PaginationModel<ConductFullDetailModel>> GetClassStudentsWithConducts(int grade, string semesterId, string classId, PageQueryModel queryModel);
        ValueTask<Guid> CreateConduct(string studentId, string semesterId);
        ValueTask UpdateConduct(Guid conductId, ConductUpdateModel model);
        ValueTask DeleteConduct(Guid conductId);
        ValueTask<ConductForClassStatisticModel> GetConductClassStatistic(int grade, string semesterId, string classId);
        ValueTask<ConductForSemesterStatisticModel> GetConductSemesterStatistic(int grade, string semesterId);
        ValueTask<ConductInSemesterModel> GetConduct(string studentId, string semesterId);
        ValueTask<ConductInSemesterModel> GetConductByClassDetailId(string classDetailId, string semesterId);
    }
}
