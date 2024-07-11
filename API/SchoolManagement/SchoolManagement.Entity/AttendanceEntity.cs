using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Entity
{
    [Table("Attendances")]
    public class AttendanceEntity
    {
        [Key]
        public Guid AttendanceId { get; set; }
        [Required]
        public DateTime LeaveDay { get; set; }
        [Required]
        public bool IsPermission { get; set; }
        public string ClassDetailId { get; set; } = string.Empty;
        public string SemesterId { get; set; } = string.Empty;

        [ForeignKey(nameof(ClassDetailId))]
        public ClassDetailEntity ClassDetail { get; set; } = null!;

        [ForeignKey(nameof(SemesterId))]
        public SemesterEntity Semester { get; set; } = null!;
    }
}
