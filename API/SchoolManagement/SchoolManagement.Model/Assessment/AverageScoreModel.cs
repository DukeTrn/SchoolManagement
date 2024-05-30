namespace SchoolManagement.Model
{
    public class AverageScoreModel
    {
        public string ClassDetailId { get; set; } = string.Empty;
        public decimal TotalAverage { get; set; }
        public List<AverageEachSubjectModel> Subjects { get; set; } = new List<AverageEachSubjectModel>();
    }
    public class AverageEachSubjectModel
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public decimal Average { get; set; }
    }

    public class AverageScoreForAcademicYearModel
    {
        public string ClassDetailId { get; set; } = string.Empty;
        public decimal TotalFirstAverage { get; set; } // Sum(FirstSemester) / count
        public decimal TotalSecondAverage { get; set; } // Sum(SecondSemester) / count
        public decimal TotalAverage { get; set; } // Sum(Average) / count
        public List<AverageEachSemesterModel> Subjects { get; set; } = new List<AverageEachSemesterModel>();
    }
    public class AverageEachSemesterModel
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public decimal FirstSemester { get; set; } // avg 1st sem
        public decimal SecondSemester { get; set; } // avg 2nd sem
        public decimal Average { get; set; } // (FirstSemester + SecondSemester*2)/3

    }
}
