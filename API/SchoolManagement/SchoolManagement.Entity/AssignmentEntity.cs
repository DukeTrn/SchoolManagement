using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Entity
{
    [Table("Assignments")]
    public class AssignmentEntity
    {
        // Phân công giảng dạy
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

        // 1-N Assign - Timetable
        public ICollection<TimetableEntity> Timetables { get; set; } = new List<TimetableEntity>();
    }
}
