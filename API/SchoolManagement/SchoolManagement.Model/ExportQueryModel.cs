using SchoolManagement.Common.Enum;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model
{
    public class ExportQueryModel
    {
        public string? SearchValue { get; set; }
        [Required]
        public int? PageSize { get; set; } = 10;
        [Required]
        public int? PageNumber { get; set; } = 1;
        public List<string> StudentIds { get; set; } = new List<string>();
        public List<StudentStatusType> Status { get; set; } = new();
    }
}
