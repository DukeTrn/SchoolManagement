using SchoolManagement.Common.Enum;

namespace SchoolManagement.Model
{
    public class TimetableCreateModel
    {
        //public Guid AssignmentId { get; set; }
        public string ClassId { get; set; } = string.Empty;
        public string SemesterId { get; set; } = string.Empty;
        public int SubjectId { get; set; }
        public DayOfWeakType DayOfWeek { get; set; }
        public PeriodType Period { get; set; }
    }

    public class TimetableUpdateModel
    {
        public DayOfWeakType DayOfWeek { get; set; }
        public PeriodType Period { get; set; }
    }
}
