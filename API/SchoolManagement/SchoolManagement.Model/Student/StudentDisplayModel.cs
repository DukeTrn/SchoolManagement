using SchoolManagement.Common.Enum;

namespace SchoolManagement.Model
{
    public class StudentDisplayModel
    {
        public string StudentId { get; set; } = string.Empty; // Khóa chính Student
        public string FullName { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public string? IdentificationNumber { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Ethnic { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public string? Email { get; set; }
        public StatusType Status { get; set; } // tình trạng học tập (đang học - đình chỉ - thôi học)
    }
}
