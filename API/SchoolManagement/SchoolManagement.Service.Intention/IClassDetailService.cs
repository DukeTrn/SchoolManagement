using SchoolManagement.Model;
using SchoolManagement.Model.ClassDetail;

namespace SchoolManagement.Service.Intention
{
    public interface IClassDetailService
    {
        Task<PaginationModel<ClassDetailDisplayModel>> GetAllClassDetail(string classId, ClassDetailQueryModel queryModel);
        Task<List<StudentFilterModel>> FilterStudentsByGrade(string academicYear, int grade);
        Task AddClassDetails(List<ClassDetailAddModel> models);
        ValueTask UpdateClassDetailAsync(string classDetailId, string newClassId);
        ValueTask DeleteClassDetail(string id);
        Task<byte[]> ExportClassDetailToExcelAsync(string classId, ClassDetailExportQueryModel queryModel);
    }
}
