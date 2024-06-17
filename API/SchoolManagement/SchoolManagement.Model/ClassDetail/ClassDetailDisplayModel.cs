using SchoolManagement.Common.Enum;

namespace SchoolManagement.Model
{
    public class ClassDetailDisplayModel
    {
        public string ClassDetailId { get; set; } = string.Empty;
        public int Grade { get; set; }
        //public string AcademicYear { get; set; } = string.Empty;
        public int Number { get; set; } //STT
        public string StudentId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public StudentStatusType Status { get; set; }
    }
}
