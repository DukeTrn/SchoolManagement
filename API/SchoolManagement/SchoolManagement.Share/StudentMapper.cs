using SchoolManagement.Entity;
using SchoolManagement.Model;

namespace SchoolManagement.Share
{
    public static class StudentMapper
    {
        public static StudentModel ToModel(this StudentEntity entity) => new()
        {
            StudentId = entity.StudentId,
            FullName = entity.FullName,
            DOB = entity.DOB,
            IdentificationNumber = entity.IdentificationNumber,
            Gender = entity.Gender,
            Address = entity.Address,
            Ethnic = entity.Ethnic,
        };
    }
}