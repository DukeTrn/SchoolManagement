namespace SchoolManagement.Model
{
    public class AssessmentDetailModel
    {
        public string ClassDetailId { get; set; } = string.Empty;
        public string StudentId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public int Grade { get; set; }
        public string AcademicYear { get; set; } = string.Empty;
    }
}
