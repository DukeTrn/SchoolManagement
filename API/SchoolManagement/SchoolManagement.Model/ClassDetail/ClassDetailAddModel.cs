using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model.ClassDetail
{
    public class ClassDetailAddModel
    {
        [Required(ErrorMessage = "Class Id is required")]
        public string ClassId { get; set; } = string.Empty;
        [Required(ErrorMessage = "Student Id is required")]
        public string StudentId { get; set; } = string.Empty;
    }
}
