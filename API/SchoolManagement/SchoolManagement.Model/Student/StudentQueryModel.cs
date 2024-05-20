using SchoolManagement.Common.Enum;

namespace SchoolManagement.Model
{
    public class StudentQueryModel : PageQueryModel
    {
        public List<StudentStatusType> Status { get; set; } = new List<StudentStatusType>();
    }
}
