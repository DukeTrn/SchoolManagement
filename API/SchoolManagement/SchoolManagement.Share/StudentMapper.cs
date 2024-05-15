using SchoolManagement.Common.Enum;
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
            Status = TranslateStatus(entity.Status),
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
            Status = TranslateStatus(entity.Status),
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

        private static string TranslateStatus(StatusType status)
        {
            switch (status)
            {
                case StatusType.Active:
                    return "Đang học";
                case StatusType.Suspended:
                    return "Đình chỉ";
                case StatusType.Inactive:
                    return "Nghỉ học";
                default:
                    return string.Empty;
            }
        }
    }
}