namespace SchoolManagement.Model
{
    public class StudentDisplayModel
    {
        public string StudentId { get; set; } = string.Empty; // Khóa chính Student
        public string FullName { get; set; } = string.Empty;
        public string DOB { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;       
        public string PhoneNumber { get; set; } = string.Empty;    
        public string? Email { get; set; }
        public string Status { get; set; } = string.Empty; // tình trạng học tập (đang học - đình chỉ - thôi học)
    }

    public class StudentFullDetailModel : StudentDisplayModel
    {
        public string? IdentificationNumber { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Ethnic { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public string FatherName { get; set; } = string.Empty;
        public string FatherJob { get; set; } = string.Empty;
        public string FatherPhoneNumber { get; set; } = string.Empty;
        public string? FatherEmail { get; set; }
        public string MotherName { get; set; } = string.Empty;
        public string MotherJob { get; set; } = string.Empty;
        public string MotherPhoneNumber { get; set; } = string.Empty;
        public string? MotherEmail { get; set; }
        public string AcademicYear { get; set; } = string.Empty; // niên khóa (2023-2026)
    }

    public class StudentFilterModel
    {
        public string StudentId { get; set; } = string.Empty; // Khóa chính
        public string FullName { get; set; } = string.Empty;
    }
}
