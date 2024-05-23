using SchoolManagement.Common.Enum;

namespace SchoolManagement.Model
{
    public class ClassDetailDisplayModel
    {
        public string StudentId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public StudentStatusType Status { get; set; }
    }
}
