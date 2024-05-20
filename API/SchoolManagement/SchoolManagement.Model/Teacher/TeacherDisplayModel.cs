namespace SchoolManagement.Model
{
    public class TeacherDisplayModel
    {
        public string TeacherId { get; set; } = string.Empty; // Khóa chính
        public string FullName { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string Level { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class TeacherFullDisplayModel : TeacherDisplayModel
    {
        public string? IdentificationNumber { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Ethnic { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public DateTime TimeStart { get; set; } // Thoi gian bat dau lam viec
        public DateTime TimeEnd { get; set; }
        public bool IsLeader { get; set; } // tổ trưởng
        public bool IsViceLeader { get; set; } // tổ phó
        //public bool NotificationIsSeen { get; set; } // đã xem
    }
}
