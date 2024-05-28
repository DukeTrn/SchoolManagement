namespace SchoolManagement.Model
{
    public class AssessmentAddModel
    {
        public string Score { get; set; } = string.Empty;
        public int Weight { get; set; } // trọng số điểm
        public string? Feedback { get; set; } = string.Empty;

        public string SemesterId { get; set; } = string.Empty;
        public int SubjectId { get; set; }
        public string ClassDetailId { get; set; } = string.Empty;
    }
    public class AssessmentUpdateModel
    {
        public string Score { get; set; } = string.Empty;
        public string? Feedback { get; set; } = string.Empty;
    }
}
