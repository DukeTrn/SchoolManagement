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
            DOB = entity.DOB.ToString("dd/MM/yyyy"),
            Gender = entity.Gender,
            PhoneNumber = entity.PhoneNumber,
            Email = entity.Email,
            Status = TranslateStatus(entity.Status),
        };

        public static StudentFullDetailModel ToFullDetailModel(this StudentEntity entity) => new()
        {
            StudentId = entity.StudentId,
            FullName = entity.FullName,
            DOB = entity.DOB.ToString("dd/MM/yyyy"),
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
            AcademicYear = entity.AcademicYear,
            UserName = entity.Account.UserName
        };

        private static string TranslateStatus(StudentStatusType status)
        {
            switch (status)
            {
                case StudentStatusType.Active:
                    return "Đang học";
                case StudentStatusType.Suspended:
                    return "Đình chỉ";
                case StudentStatusType.Inactive:
                    return "Nghỉ học";
                case StudentStatusType.Graduated:
                    return "Tốt nghiệp";
                default:
                    return string.Empty;
            }
        }
    }
}