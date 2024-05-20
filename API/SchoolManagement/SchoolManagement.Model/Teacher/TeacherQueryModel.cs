using SchoolManagement.Common.Enum;

namespace SchoolManagement.Model
{
    public class TeacherQueryModel : PageQueryModel
    {
        public List<TeacherStatusType> Status { get; set; } = new List<TeacherStatusType>();
    }
}
