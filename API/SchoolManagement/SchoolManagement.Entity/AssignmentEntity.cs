using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Entity
{
    [Table("Assignments")]
    public class AssignmentEntity
    {
        // Phan cong giang day
        [Key]
        public Guid AssignmentId { get; set; }
        
        // Foreign key
        public string SemesterId { get; set; } = string.Empty;

        // Foreign key
        public string TeacherId { get; set; } = string.Empty;

        //Foreign key
        public int SubjectId { get; set; }

        // Foreign key
        public string ClassId { get; set; } = string.Empty;

        public SemesterEntity Semester { get; set; } = null!;
        public TeacherEntity Teacher { get; set; } = null!;
        public SubjectEntity Subject { get; set; } = null!;
        public ClassEntity Class { get; set; } = null!;
    }
}
