using Microsoft.AspNetCore.Http;
using SchoolManagement.Common.Enum;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model
{
    public class TeacherAddModel
    {
        [Required(ErrorMessage = "TeacherId is required")]
        public string TeacherId { get; set; } = string.Empty;
        [Required(ErrorMessage = "FullName is required")]
        public string FullName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime DOB { get; set; }
        public string? IdentificationNumber { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; } = string.Empty;
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; } = string.Empty;
        [Required(ErrorMessage = "Ethnic is required")]
        public string Ethnic { get; set; } = string.Empty;
        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; } = string.Empty;
        public IFormFile? Avatar { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;
        public DateTime TimeStart { get; set; } // Thoi gian bat dau lam viec
        public DateTime? TimeEnd { get; set; }
        [Required(ErrorMessage = "Level is required")]
        public string Level { get; set; } = string.Empty; // bằng cấp
        //public bool IsLeader { get; set; } // tổ trưởng
        //public bool IsViceLeader { get; set; } // tổ phó
        //public bool NotificationIsSeen { get; set; } // đã xem
        //public TeacherStatusType Status { get; set; }
    }
}
