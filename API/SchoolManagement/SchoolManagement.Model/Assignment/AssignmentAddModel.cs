namespace SchoolManagement.Model
{
    public class AssignmentAddModel
    {
        public string SemesterId { get; set; } = string.Empty;
        public string TeacherId { get; set; } = string.Empty;
        public int SubjectId { get; set; }
        public string ClassId { get; set; } = string.Empty;
    }
}
