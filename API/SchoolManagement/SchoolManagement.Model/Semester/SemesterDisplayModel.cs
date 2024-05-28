namespace SchoolManagement.Model
{
    public class SemesterDisplayModel
    {
        public string SemesterId { get; set; } = string.Empty;
        public string SemesterName { get; set; } = string.Empty; // nam hoc 2024
        public string AcademicYear { get; set; } = string.Empty; // nien khoa 2024-2025
        public string TimeStart { get; set; } = string.Empty;
        public string? TimeEnd { get; set; } = string.Empty;
    }

    public class SemesterFilterModel
    {
        public string SemesterId { get; set; } = string.Empty;
        public string SemesterName { get; set; } = string.Empty;
    }
}
