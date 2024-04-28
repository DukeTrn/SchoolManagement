using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Entity
{
    [Table("Conducts")]
    public class ConductEntity
    {
        [Key]
        public Guid ConductId { get; set; }
        public int ConductName { get; set; } // Enum: very good, good, average, weak
        public string Feedback { get; set; } = string.Empty;

        // Khóa ngoại cho mối quan hệ 1-N với StudentEntity
        public string StudentId { get; set; } = string.Empty;

        // Khóa ngoại cho mối quan hệ 1-N với SemesterEntity
        public string SemesterId { get; set; } = string.Empty;

        public StudentEntity Student { get; set; } = null!;
        public SemesterEntity Semester { get; set; } = null!;
    }
}
