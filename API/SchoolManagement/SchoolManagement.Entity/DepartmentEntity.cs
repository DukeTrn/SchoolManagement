using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Entity
{
    [Table("Departments")]
    public class DepartmentEntity
    {
        // tổ bộ môn
        [Key]
        public string DepartmentId { get; set; } = string.Empty; // khóa chính
        public string SubjectName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Notification { get; set; } = string.Empty;

        // 1-N Department-Teachers
        public ICollection<TeacherEntity> Teachers { get; set; } = null!;
    }
}
