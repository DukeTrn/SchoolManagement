using SchoolManagement.Common.Enum;

namespace SchoolManagement.Model
{
    public class ConductUpdateModel
    {
        public ConductType ConductType { get; set; }
        public string? Feedback { get; set; } = string.Empty;

    }
}
