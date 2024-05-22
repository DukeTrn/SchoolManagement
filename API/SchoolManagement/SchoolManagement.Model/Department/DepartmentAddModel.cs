using SchoolManagement.Common.Enum;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model
{
    public class DepartmentAddModel
    {
        [Required(ErrorMessage = "Department Id is required")]
        public string DepartmentId { get; set; } = string.Empty;
        [Required(ErrorMessage = "Department Name is required")]
        public string SubjectName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Notification { get; set; } = string.Empty;
    }

    public class UpdateTeachersToDeptModel
    {
        public string DepartmentId { get; set; } = string.Empty;
        public List<string> TeacherIds { get; set; } = new List<string>();
    }

    public class PromoteTeacherModel
    {
        public string DepartmentId { get; set; } = string.Empty;
        public string HeadId { get; set; } = string.Empty;
        public string FirstDeputyId { get; set; } = string.Empty;
        public string SecondDeputyId { get; set; } = string.Empty;
    }
}
