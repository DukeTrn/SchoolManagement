using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Entity
{
    [Table("ClassDetails")]
    public class ClassDetailEntity
    {
        [Key]
        public string ClassDetailId { get; set; } = string.Empty;
        public string? ClassId { get; set; } = string.Empty;
        public string? StudentId { get; set; } = string.Empty;
        public DateTime? ModifiedAt { get; set; }

        // Navigation properties 
        public ClassEntity? Class { get; set; } = null!;
        public StudentEntity? Student { get; set; } = null!;

        // 1-N ClassDetail-Assessments
        public ICollection<AssessmentEntity> Assessments { get; set; } = null!;
    }
}
