using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model
{
    public class ClassUpdateModel
    {
        [Required(ErrorMessage = "Class Name is required")]
        public string ClassName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Teacher Id is required")]
        public string HoomroomTeacherId { get; set; } = string.Empty;
    }
}
