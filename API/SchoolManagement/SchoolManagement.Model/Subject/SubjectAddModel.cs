namespace SchoolManagement.Model
{
    public class SubjectAddModel
    {
        public string SubjectName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int Grade { get; set; }
    }

    public class SubjectUpdateModel
    {
        public string SubjectName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int Grade { get; set; }
    }
}
