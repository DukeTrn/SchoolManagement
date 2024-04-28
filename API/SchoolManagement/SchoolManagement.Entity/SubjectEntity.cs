using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Entity
{
    [Table("Subjects")]
    public class SubjectEntity
    {
        [Key]
        public int SubjectId { get; set; } // khóa chính , maybe using Enum
        public string SubjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Grade { get; set; }

        // 1-N Subject-Assignments
        public ICollection<AssignmentEntity> Assignments { get; set; } = null!;

        // 1-N Subject-Assessments
        public ICollection<AssessmentEntity> Assessments { get; set; } = null!;
    }
}
