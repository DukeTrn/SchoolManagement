using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model
{
    public class ClassAddModel
    {
        [Required(ErrorMessage = "Class Name is required")]
        public string ClassName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Year is required")]
        public string Year { get; set; } = string.Empty;
        [Required(ErrorMessage = "Grade is required")]
        public int Grade { get; set; }
        [Required(ErrorMessage = "Teacher Id is required")]
        public string HomeroomTeacherId { get; set; } = string.Empty;
    }
}
