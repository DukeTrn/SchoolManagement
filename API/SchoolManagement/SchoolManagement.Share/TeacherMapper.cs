using SchoolManagement.Common.Enum;
using SchoolManagement.Entity;
using SchoolManagement.Model;

namespace SchoolManagement.Share
{
    public static class TeacherMapper
    {
        public static TeacherDisplayModel ToModel(this TeacherEntity entity) => new()
        {
            TeacherId = entity.TeacherId,
            FullName = entity.FullName,
            DOB = entity.DOB,
            Gender = entity.Gender,
            PhoneNumber = entity.PhoneNumber,
            Email = entity.Email,
            Level = entity.Level,
            Status = TranslateStatus(entity.Status)
        };

        public static TeacherFullDisplayModel ToFullDetailModel(this TeacherEntity entity) => new()
        {
            TeacherId = entity.TeacherId,
            FullName = entity.FullName,
            DOB = entity.DOB,
            IdentificationNumber = entity.IdentificationNumber,
            Gender = entity.Gender,
            Address = entity.Address,
            Ethnic = entity.Ethnic,
            PhoneNumber = entity.PhoneNumber,
            Avatar = entity.Avatar,
            Email = entity.Email,
            Level = entity.Level,
            TimeStart = entity.TimeStart,
            TimeEnd = entity.TimeEnd,
            Status = TranslateStatus(entity.Status),

        };

        private static string TranslateStatus(TeacherStatusType status)
        {
            switch (status)
            {
                case TeacherStatusType.Active:
                    return "Đang giảng dạy";
                case TeacherStatusType.Suspended:
                    return "Tạm nghỉ";
                case TeacherStatusType.Inactive:
                    return "Nghỉ việc";
                default:
                    return string.Empty;
            }
        }
    }
}
