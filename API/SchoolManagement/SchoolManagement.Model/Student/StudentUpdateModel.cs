namespace SchoolManagement.Model
{
    public class StudentUpdateModel
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public string? IdentificationNumber { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Ethnic { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public string? Email { get; set; }
        //public StatusType Status { get; set; } // tình trạng học tập (đang học - đình chỉ - thôi học)

        // Parent information
        public string FatherName { get; set; } = string.Empty;
        public string FatherJob { get; set; } = string.Empty;
        public string FatherPhoneNumber { get; set; } = string.Empty;
        public string? FatherEmail { get; set; }
        public string MotherName { get; set; } = string.Empty;
        public string MotherJob { get; set; } = string.Empty;
        public string MotherPhoneNumber { get; set; } = string.Empty;
        public string? MotherEmail { get; set; }
    }
}
