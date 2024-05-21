using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Entity
{
    [Table("Assessments")]
    public class AssessmentEntity
    {
        [Key]
        public Guid AssessmentId { get; set; }
        public string Score { get; set; } = string.Empty;
        public int Weight { get; set; } // trọng số điểm
        public string Feedback { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        // Foreign key
        public string SemesterId { get; set; } = string.Empty;
        public int SubjectId { get; set; }
        public string ClassDetailId { get; set; } = string.Empty;

        public SemesterEntity Semester { get; set; } = null!;
        public SubjectEntity Subject { get; set; } = null!;
        public ClassDetailEntity ClassDetail { get; set; } = null!;
    }
}
