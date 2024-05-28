namespace SchoolManagement.Model
{ 
    public class SemesterDetailDisplayModel
    {
        public Guid Id { get; set; }
        public string ClassId { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string HomeroomTeacherId { get; set; } = string.Empty; // GVCN
        public string HomeroomTeacherName { get; set; } = string.Empty;
        public int Grade { get; set; }
        public string AcademicYear { get; set; } = string.Empty; // Nam hoc (Class Entity)
    }
}
