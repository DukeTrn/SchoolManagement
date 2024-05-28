using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Entity
{
    [Table("SemesterDetails")]
    public class SemesterDetailEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string? SemesterId { get; set; }
        public string? ClassId { get; set; }

        // Navigation properties
        public ClassEntity? Class { get; set; } = null!;
        public SemesterEntity? Semester { get; set; } = null!;
    }
}
