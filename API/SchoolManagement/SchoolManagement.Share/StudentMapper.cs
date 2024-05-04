using SchoolManagement.Entity;
using SchoolManagement.Model;

namespace SchoolManagement.Share
{
    public static class StudentMapper
    {
        public static StudentDisplayModel ToModel(this StudentEntity entity) => new()
        {
            StudentId = entity.StudentId,
            FullName = entity.FullName,
            DOB = entity.DOB,
            IdentificationNumber = entity.IdentificationNumber,
            Gender = entity.Gender,
            Address = entity.Address,
            Ethnic = entity.Ethnic,
            PhoneNumber = entity.PhoneNumber,
            Avatar = entity.Avatar,
            Email = entity.Email,
            Status = entity.Status,
        };

        public static StudentFullDetailModel ToFullDetailModel(this StudentEntity entity) => new()
        {
            StudentId = entity.StudentId,
            FullName = entity.FullName,
            DOB = entity.DOB,
            IdentificationNumber = entity.IdentificationNumber,
            Gender = entity.Gender,
            Address = entity.Address,
            Ethnic = entity.Ethnic,
            PhoneNumber = entity.PhoneNumber,
            Avatar = entity.Avatar,
            Email = entity.Email,
            Status = entity.Status,
            FatherName = entity.FatherName,
            FatherJob = entity.FatherJob,
            FatherPhoneNumber = entity.FatherPhoneNumber,
            FatherEmail = entity.FatherEmail,
            MotherName = entity.MotherName,
            MotherJob = entity.MotherJob,
            MotherPhoneNumber = entity.MotherPhoneNumber,
            MotherEmail = entity.MotherEmail,
            AcademicYear = entity.AcademicYear
        };
    }
}