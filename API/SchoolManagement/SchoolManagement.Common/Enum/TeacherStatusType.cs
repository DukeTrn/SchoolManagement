using System.ComponentModel;

namespace SchoolManagement.Common.Enum
{
    public enum TeacherStatusType
    {
        [Description("Đang giảng dạy")]
        Active = 1,
        [Description("Tạm nghỉ")]
        Suspended = 2,
        [Description("Nghỉ việc")]
        Inactive = 3,
    }
}
