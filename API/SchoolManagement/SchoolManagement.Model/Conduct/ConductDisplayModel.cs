namespace SchoolManagement.Model
{
    public class ConductDisplayModel
    {
        public string ClassName { get; set; } = string.Empty; // from class entity
        public string AcademicYear { get; set; } = string.Empty; // from semester entity
        public int TotalStudents { get; set; } // get total students in 1 class in class detail entity
        public string HomeroomTeacherName { get; set; } = string.Empty; // from class entity
    }

    public class ConductFullDetailModel 
    {
        public string ClassDetailId { get; set; } = string.Empty;
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;   
        public Guid? ConductId { get; set; }
        public string? ConductName { get; set; } = string.Empty;
        public string? Feedback { get; set; } = string.Empty;
    }
}
