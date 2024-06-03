namespace SchoolManagement.Model
{
    public class AssessmentForClassStatisticModel
    {
        public int TotalStudent { get; set; }

        // For mark in [8, 10]
        public decimal VeryGoodCount { get; set; }
        public string VeryGoodPercentage { get; set; } = string.Empty;

        // For mark in [6.5, 8)
        public decimal GoodCount { get; set; }
        public string GoodPercentage { get; set; } = string.Empty;

        // For mark in [5, 6.5)
        public decimal AverageCount { get; set; }
        public string AveragePercentage { get; set; } = string.Empty;

        // For mark in [3.5, 5)
        public decimal WeakCount { get; set; }
        public string WeakPercentage { get; set; } = string.Empty;

        // For mark in [0, 3.5)
        public decimal PoorCount { get; set; }
        public string PoorPercentage { get; set; } = string.Empty;
    }
}
