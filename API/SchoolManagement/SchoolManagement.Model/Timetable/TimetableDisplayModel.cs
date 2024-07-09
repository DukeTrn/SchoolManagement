using SchoolManagement.Common.Enum;

namespace SchoolManagement.Model
{
    public class TimetableDisplayModel
    {
        public Guid TimetableId { get; set; }
        public Guid AssignmentId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public DayOfWeakType DayOfWeek { get; set; }
        public PeriodType Period { get; set; }
    }
}
