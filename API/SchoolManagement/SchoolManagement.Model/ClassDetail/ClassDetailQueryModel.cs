using SchoolManagement.Common.Enum;

namespace SchoolManagement.Model
{
    public class ClassDetailQueryModel : PageQueryModel
    {
        public List<StudentStatusType> Status { get; set; } = new List<StudentStatusType>();
    }
}
