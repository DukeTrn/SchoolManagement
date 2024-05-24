using SchoolManagement.Common.Enum;

namespace SchoolManagement.Model
{
    public class ClassDetailExportQueryModel : PageQueryModel
    {
        public List<string> StudentIds { get; set; } = new List<string>();
        public List<StudentStatusType> Status { get; set; } = new();
    }
}
