using SchoolManagement.Model.ClassDetail;

namespace SchoolManagement.Service.Intention
{
    public interface IClassDetailService
    {
        Task AddClassDetails(List<ClassDetailAddModel> models);
    }
}
