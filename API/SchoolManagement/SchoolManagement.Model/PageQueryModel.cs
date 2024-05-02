using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model
{
    public class PageQueryModel
    {
        public string? SearchValue { get; set; }
        [Required]
        public int? PageSize { get; set; } = 10;
        [Required]
        public int? PageNumber { get; set; } = 1;
    }
}
