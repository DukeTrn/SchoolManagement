using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model.Semester
{
    public class SemesterAddModel
    {
        [Required(ErrorMessage = "Semester Id is required")]
        public string SemesterId { get; set; } = string.Empty;
        [Required(ErrorMessage = "Semester Name is required")]
        public string SemesterName { get; set; } = string.Empty; // nam hoc 2024
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
    }

    public class SemesterUpdateModel
    {
        [Required(ErrorMessage = "Semester Name is required")]
        public string SemesterName { get; set; } = string.Empty; // nam hoc 2024
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
    }
}
