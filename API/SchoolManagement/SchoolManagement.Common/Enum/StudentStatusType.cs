using System.ComponentModel;
namespace SchoolManagement.Common.Enum
{
    public enum StudentStatusType
    {
        [Description("Đang học")]
        Active = 1,
        [Description("Đình chỉ")]
        Suspended = 2,
        [Description("Nghỉ học")]
        Inactive = 3,
        [Description("Tốt nghiệp")]
        Graduated = 4,
    }
}
