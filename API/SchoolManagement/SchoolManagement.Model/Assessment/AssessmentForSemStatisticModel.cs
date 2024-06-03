namespace SchoolManagement.Model
{
    public class AssessmentForSemStatisticModel
    {
        public int TotalStudents { get; set; }
        public string VeryGoodPercentage { get; set; } = string.Empty;
        public string GoodPercentage { get; set; } = string.Empty;
        public string AveragePercentage { get; set; } = string.Empty;
        public string WeakPercentage { get; set; } = string.Empty;
        public string PoorPercentage { get; set; } = string.Empty;
    }
}
