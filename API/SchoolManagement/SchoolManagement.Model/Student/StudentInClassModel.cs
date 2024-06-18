namespace SchoolManagement.Model.Student
{
    public class StudentInClassModel
    {
        public string ClassDetailId { get; set; } = string.Empty;
        public string ClassId { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string AcademicYear { get; set; } = string.Empty;
        public int Grade { get; set; }
        public int TotalStudents { get; set; }
        public string HomeroomTeacherId { get; set; } = string.Empty; // GVCN
        public string HomeroomTeacherName { get; set; } = string.Empty; // GVCN
    }
}
