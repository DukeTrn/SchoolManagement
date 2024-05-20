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
}
