namespace SchoolManagement.Model
{
    public class ClassDisplayModel
    {
        public string ClassId { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string AcademicYear { get; set; } = string.Empty;
        public int Grade { get; set; } // khối
        public string HomeroomTeacherId { get; set; } = string.Empty; // GVCN
        public string HomeroomTeacherName { get; set; } = string.Empty; // GVCN
    }

    public class HomeroomClassDisplayModel
    {
        public string ClassId { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string AcademicYear { get; set; } = string.Empty;
        public int Grade { get; set; } // khối
        public int TotalStudents { get; set; }
    }
    public class NormalClassDisplayModel : HomeroomClassDisplayModel 
    { 
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
    }

    public class ClassFilterModel
    {
        public string ClassId { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
    }
}
