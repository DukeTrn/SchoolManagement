namespace SchoolManagement.Model
{
    public class ConductDisplayModel
    {
        public string ClassName { get; set; } = string.Empty;
        public string AcademicYear { get; set; } = string.Empty;
        public int TotalStudents { get; set; }
        public string HomeroomTeacherName { get; set; } = string.Empty;
    }

    public class ConductFullDetailModel
    {
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;   
        public string ConductName { get; set; } = string.Empty;
        public string Feedback { get; set; } = string.Empty;
    }
}
