using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Entity
{
    [Table("Teachers")]
    public class TeacherEntity
    {
        [Key]
        public string TeacherId { get; set; } = string.Empty; // Khóa chính
        public string FullName { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public string? IdentificationNumber { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Ethnic { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public string? Email { get; set; }
        public DateTime TimeStart { get; set; } // Thoi gian bat dau lam viec
        public DateTime TimeEnd { get; set; }
        public string Level { get; set; } = string.Empty; // bằng cấp
        public bool IsLeader { get; set; } // tổ trưởng
        public bool IsViceLeader { get; set; } // tổ phó
        //public bool NotificationIsSeen { get; set; } // đã xem
        public int Status { get; set; } // tình trạng làm việc: đang dạy, tạm nghỉ, nghỉ việc

        // 1-1 teacher-user
        public Guid AccountId { get; set; } // Khóa ngoại tham chiếu đến UserEntity
        public AccountEntity Account { get; set; } = null!;

        // 1-N Department-Teacher
        public string DepartmentId { get; set; } = string.Empty;
        public DepartmentEntity Department { get; set; } = null!;

        // 1-1 HomeroomTeacher-Class
        public string? ClassId { get; set; } = null;
        public ClassEntity Class { get; set; } = null!;

        // 1-N Teacher-Assignments
        public ICollection<AssignmentEntity> Assignments { get; set; } = null!;
    }
}
