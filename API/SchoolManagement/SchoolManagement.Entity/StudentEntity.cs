using SchoolManagement.Common.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Entity
{
    [Table("Students")]
    public class StudentEntity 
    {
        [Key]
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
        public StudentStatusType Status { get; set; } // tình trạng học tập (đang học - đình chỉ - thôi học)

        // Parent information
        public string FatherName { get; set; } = string.Empty;
        public string FatherJob { get; set; } = string.Empty;
        public string FatherPhoneNumber { get; set; } = string.Empty;
        public string? FatherEmail { get; set; }
        public string MotherName { get; set; } = string.Empty;
        public string MotherJob { get; set; } = string.Empty;
        public string MotherPhoneNumber { get; set; } = string.Empty;
        public string? MotherEmail { get; set; }
        public string AcademicYear { get; set; } = string.Empty; // niên khóa (2023-2026)       

        // 1-1 student-user
        public Guid? AccountId { get; set; } // Khóa ngoại tham chiếu đến AccountEntity
        public AccountEntity? Account { get; set; } = null!;

        // 1-N Student-ClassDetails
        public ICollection<ClassDetailEntity> ClassDetails { get; set; } = null!;

        // 1-N Student-Conducts
        public ICollection<ConductEntity> Conducts { get; set; } = null!;
    }
}
