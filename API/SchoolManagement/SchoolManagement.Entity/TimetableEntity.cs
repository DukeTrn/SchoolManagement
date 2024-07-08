using SchoolManagement.Common.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Entity
{
    [Table("Timetables")]
    public class TimetableEntity
    {
        [Key]
        public Guid TimetableId { get; set; }

        [Required]
        public Guid AssignmentId { get; set; }

        [Required]
        public DayOfWeakType DayOfWeek { get; set; } // Ex: Monday

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [ForeignKey(nameof(AssignmentId))]
        public AssignmentEntity Assignment { get; set; } = null!;
    }
}
