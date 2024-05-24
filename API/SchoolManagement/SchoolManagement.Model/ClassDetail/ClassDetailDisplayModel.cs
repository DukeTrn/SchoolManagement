using SchoolManagement.Common.Enum;

namespace SchoolManagement.Model
{
    public class ClassDetailDisplayModel
    {
        public string ClassDetailId { get; set; } = string.Empty;
        public int Number { get; set; }
        public string FullName { get; set; } = string.Empty;
        public StudentStatusType Status { get; set; }
    }
}
