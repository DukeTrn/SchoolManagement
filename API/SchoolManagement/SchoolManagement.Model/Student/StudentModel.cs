namespace SchoolManagement.Model
{
    public class StudentModel
    {
        public string StudentId { get; set; } = string.Empty; // Khóa chính Student
        public string FullName { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public string? IdentificationNumber { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Ethnic { get; set; } = string.Empty;
    }
}
