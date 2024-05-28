namespace SchoolManagement.Model
{
    public class SemesterDetailQueryModel : PageQueryModel
    {
        public List<int> Grades { get; set; } = new List<int>();
    }
}
