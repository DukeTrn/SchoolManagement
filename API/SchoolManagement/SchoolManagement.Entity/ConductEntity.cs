using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Entity
{
    [Table("Conducts")]
    public class ConductEntity
    {
        // Hạnh kiểm
        [Key]
        public Guid ConductId { get; set; }
        public int ConductName { get; set; } // Enum: very good (tốt), good (khá), average (TB), weak (yếu)
        public string Feedback { get; set; } = string.Empty;

        // Khóa ngoại cho mối quan hệ 1-N với StudentEntity
        public string StudentId { get; set; } = string.Empty;

        // Khóa ngoại cho mối quan hệ 1-N với SemesterEntity
        public string SemesterId { get; set; } = string.Empty;

        public StudentEntity Student { get; set; } = null!;
        public SemesterEntity Semester { get; set; } = null!;
    }
}
