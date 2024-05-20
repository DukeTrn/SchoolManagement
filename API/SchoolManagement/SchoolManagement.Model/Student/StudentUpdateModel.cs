using Microsoft.AspNetCore.Http;
using SchoolManagement.Common.Enum;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model
{
    public class StudentUpdateModel
    {
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
        public string? Email { get; set; }
        public StudentStatusType Status { get; set; } // tình trạng học tập (đang học - đình chỉ - thôi học)

        // Parent information
        [Required(ErrorMessage = "Father name is required")]
        public string FatherName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Father job is required")]
        public string FatherJob { get; set; } = string.Empty;
        [Required(ErrorMessage = "Father phone number is required")]
        public string FatherPhoneNumber { get; set; } = string.Empty;
        public string? FatherEmail { get; set; }
        [Required(ErrorMessage = "Mother name is required")]
        public string MotherName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Mother job is required")]
        public string MotherJob { get; set; } = string.Empty;
        [Required(ErrorMessage = "Mother phone number is required")]
        public string MotherPhoneNumber { get; set; } = string.Empty;
        public string? MotherEmail { get; set; }
    }
}
