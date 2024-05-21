using SchoolManagement.Common.Enum;

namespace SchoolManagement.Model
{
    public class TeacherExportQueryModel : PageQueryModel
    {
        public List<string> TeacherIds { get; set; } = new List<string>();
        public List<TeacherStatusType> Status { get; set; } = new();
    }
}
