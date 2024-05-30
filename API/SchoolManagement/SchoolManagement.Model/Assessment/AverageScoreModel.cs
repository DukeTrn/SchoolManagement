using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model.Assessment
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
}
