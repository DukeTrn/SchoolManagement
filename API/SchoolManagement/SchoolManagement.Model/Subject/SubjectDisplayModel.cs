namespace SchoolManagement.Model
{
    public class SubjectDisplayModel
    {
        public int Id { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int Grade { get; set; }
    }
}
