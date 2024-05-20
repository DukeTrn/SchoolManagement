using SchoolManagement.Entity;
using SchoolManagement.Model;

namespace SchoolManagement.Share
{
    public static class SemesterMapper
    {
        public static SemesterDisplayModel ToModel(this SemesterEntity e) => new()
        {
            SemesterId = e.SemesterId,
            SemesterName = e.SemesterName,
            AcademicYear = e.AcademicYear,
            TimeStart = e.TimeStart.ToString("dd/MM/yyyy"),
            TimeEnd = e.TimeEnd ==  null ? "" : e.TimeEnd.Value.ToString("dd/MM/yyyy"),
        };
    }
}
