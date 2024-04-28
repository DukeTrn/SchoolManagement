using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Entity
{
    [Table("Accounts")]
    public class AccountEntity
    {
        [Key]
        public Guid AccountId { get; set; } // Khóa chính cho AccountEntity
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool IsActive { get; set; }
        public int Status { get; set; }

        // 1-1 Accout-User
        public UserEntity User { get; set; } = null!;
    }

    [Table("Users")]
    public class UserEntity
    {
        [Key]
        public Guid UserId { get; set; } // Khóa chính cho UserEntity
        public string FullName { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public string? IdentificationNumber { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Ethnic { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public string? Email { get; set; }
        // Khóa ngoại AccountEntity
        public Guid AccountId { get; set; }

        // 1-1 User-Account
        public AccountEntity Account { get; set; } = null!;

        // 1-1 User-Student
        public StudentEntity Student { get; set; } = null!;

        // 1-1 User-Teacher
        public TeacherEntity Teacher { get; set; } = null!;
    }

}