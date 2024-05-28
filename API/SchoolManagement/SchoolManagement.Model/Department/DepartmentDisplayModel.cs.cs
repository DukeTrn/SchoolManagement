namespace SchoolManagement.Model
{
    public class DepartmentDisplayModel
    {
        public string DepartmentId { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Notification { get; set; } = string.Empty;
    }

    public class DepartmentFilterModel
    {
        public string DepartmentId { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
    }
}
