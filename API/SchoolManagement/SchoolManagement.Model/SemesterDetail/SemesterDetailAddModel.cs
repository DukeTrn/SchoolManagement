using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model
{
    public class SemesterDetailAddModel
    {
        [Required(ErrorMessage = "Semester Id is required")]
        public string SemesterId { get; set; } = string.Empty;
        [Required(ErrorMessage = "Class Id is required")]
        public List<string> ClassId { get; set; } = new List<string>();
    }
}
