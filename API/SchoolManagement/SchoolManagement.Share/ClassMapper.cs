using SchoolManagement.Entity;
using SchoolManagement.Model;

namespace SchoolManagement.Share
{
    public static class ClassMapper
    {
        public static ClassDisplayModel ToModel(this ClassEntity e) => new()
        {
            ClassId = e.ClassId,
            ClassName = e.ClassName,
            AcademicYear = e.AcademicYear,
            Grade = e.Grade,
            HomeroomTeacherId = e.HomeroomTeacherId,
            HomeroomTeacherName = e.HomeroomTeacher.FullName
        };
    }
}
