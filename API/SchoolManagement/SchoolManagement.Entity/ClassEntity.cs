using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Entity
{
    [Table("Classes")]
    public class ClassEntity
    {
        [Key]
        public string ClassId { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string AcademicYear { get; set; } = string.Empty; // 2024
        public int Grade { get; set; } // khối
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        // Foreign key 
        public string HomeroomTeacherId { get; set; } = string.Empty; // GVCN
        public TeacherEntity HomeroomTeacher { get; set; } = null!;

        // 1-N Class-ClassDetail
        public ICollection<ClassDetailEntity> ClassDetails { get; set; } = null!;

        // 1-N Class-Assignments
        public ICollection<AssignmentEntity> Assignments { get; set; } = null!;

        // 1-N Class-SemClass
        public ICollection<SemesterDetailEntity> SemClassIds { get; set; } = null!;
    }
}
