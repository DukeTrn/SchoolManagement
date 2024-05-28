namespace SchoolManagement.Model
{
    public class ConductForClassStatisticModel
    {
        public int TotalStudent { get; set; }
        public decimal VeryGoodCount { get; set; }
        public string VeryGoodPercentage { get; set; } = string.Empty;

        public decimal GoodCount { get; set; }
        public string GoodPercentage { get; set; } = string.Empty;

        public decimal AverageCount { get; set; }
        public string AveragePercentage { get; set; } = string.Empty;

        public decimal WeakCount { get; set; }
        public string WeakPercentage { get; set; } = string.Empty;
    }
}
