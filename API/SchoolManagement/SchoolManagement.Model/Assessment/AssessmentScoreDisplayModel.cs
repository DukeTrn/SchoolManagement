namespace SchoolManagement.Model.Assessment
{
    public class AssessmentScoreDisplayModel
    {
        public string ClassDetailId { get; set; } = string.Empty;            
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public List<ScoreModel> Weight1 { get; set; } = new List<ScoreModel>();
        public List<ScoreModel> Weight2 { get; set; } = new List<ScoreModel>();
        public List<ScoreModel> Weight3 { get; set; } = new List<ScoreModel>();

    }
    public class ScoreModel
    {
        public Guid AssessmentId { get; set; }
        public string Score { get; set; } = string.Empty;
        public string? Feedback { get; set; }
    }

}
