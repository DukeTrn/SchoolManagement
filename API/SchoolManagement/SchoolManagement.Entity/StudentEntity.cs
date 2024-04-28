using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Entity
{
    [Table("Students")]
    public class StudentEntity 
    {
        [Key]
        public string StudentId { get; set; } = string.Empty; // Khóa chính Student
        public string FatherName { get; set; } = string.Empty;
        public string FatherJob { get; set; } = string.Empty;
        public string FatherPhoneNumber { get; set; } = string.Empty;
        public string? FatherEmail { get; set; }
        public string MotherName { get; set; } = string.Empty;
        public string MotherJob { get; set; } = string.Empty;
        public string MotherPhoneNumber { get; set; } = string.Empty;
        public string? MotherEmail { get; set; }
        public string AcademicYear { get; set; } = string.Empty; // niên khóa (2023-2026)
        public int Status { get; set; } // tình trạng học tập (đang học - đình chỉ - thôi học)

        // 1-1 student-user
        public Guid UserId { get; set; } // Khóa ngoại tham chiếu đến UserEntity
        public UserEntity User { get; set; } = null!;

        // 1-N Student-ClassDetails
        public ICollection<ClassDetailEntity> ClassDetails { get; set; } = null!;

        // 1-N Student-Conducts
        public ICollection<ConductEntity> Conducts { get; set; } = null!;
    }
}
