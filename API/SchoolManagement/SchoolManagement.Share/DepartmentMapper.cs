using SchoolManagement.Entity;
using SchoolManagement.Model.Department;

namespace SchoolManagement.Share
{
    public static class DepartmentMapper
    {
        public static DepartmentDisplayModel ToModel(this DepartmentEntity e) => new()
        {
            DepartmentId = e.DepartmentId,
            SubjectName = e.SubjectName,
            Description = e.Description,
            Notification = e.Notification,
        };
    }
}
