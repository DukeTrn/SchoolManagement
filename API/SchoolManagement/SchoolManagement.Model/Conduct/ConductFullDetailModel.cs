namespace SchoolManagement.Model
{
    public class ConductFullDetailModel 
    {
        public string ClassDetailId { get; set; } = string.Empty;
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;   
        public Guid? ConductId { get; set; }
        public string? ConductName { get; set; } = string.Empty;
        public string? Feedback { get; set; } = string.Empty;
    }
}
