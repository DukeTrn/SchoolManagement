namespace SchoolManagement.Model
{
    public class ClassInSemesterModel
    {
        public string ClassName { get; set; } = string.Empty; // from class entity
        public string AcademicYear { get; set; } = string.Empty; // from semester entity
        public int TotalStudents { get; set; } // get total students in 1 class in class detail entity
        public string HomeroomTeacherName { get; set; } = string.Empty; // from class entity
    }
}
