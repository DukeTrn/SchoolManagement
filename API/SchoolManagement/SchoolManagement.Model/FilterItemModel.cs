using SchoolManagement.Common.Attributes;
using SchoolManagement.Common.Enum;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Model
{
    public class FilterItemModel
    {
        [Required]
        public string KeyName { get; set; } = null!;

        [NotEmpty]
        public IEnumerable<string> Values { get; set; } = null!;
    }

    public class FilterOperatorItemModel
    {
        public string? KeyName { get; set; }
        public System.Collections.IList Values { get; set; } = null!;
        public FilterOperator Operator { get; set; }
        public Type KeyType { get; set; } = null!;
    }
}
