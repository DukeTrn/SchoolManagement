using Microsoft.AspNetCore.Identity;
using SchoolManagement.Common.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Entity
{
    [Table("Accounts")]
    public class AccountEntity
    { 
        [Key]
        public Guid AccountId { get; set; } // Khóa chính cho AccountEntity
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public string PasswordHashed { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ResetToken { get; set; }
        public bool IsActive { get; set; }
        public RoleType Role { get; set; } // enum 

        // 1-1 User-Student
        public StudentEntity Student { get; set; } = null!;

        // 1-1 User-Teacher
        public TeacherEntity Teacher { get; set; } = null!;
    }
}