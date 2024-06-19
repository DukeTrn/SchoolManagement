using SchoolManagement.Common.Enum;

namespace SchoolManagement.Model
{
    public class AverageScoreModel
    {
        public string ClassDetailId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public Guid ConductId { get; set; }
        public ConductType? ConductName { get; set; }
        // nếu TotalAverage >= 8 && Average của từng Subject trong Subjects >=6.5 && (SubjectId == 1 >= 8 || SubjectId == 12 >= 8) ==> AcademicPerform == "Giỏi"
        public string AcademicPerform { get; set; } = string.Empty; // [0;3.5): Kém ; [3.5;5): Yếu ; [5;6.5): Trung bình ; [6.5;8): Khá ; [8;10]: Giỏi
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
        public string Title { get; set; } = string.Empty;
        public ConductType? ConductName { get; set; }
        public string AcademicPerform { get; set; } = string.Empty;
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
