using SchoolManagement.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class ResponseModel
    {
        public string? TraceParentId { get; set; } = null!;
        public bool Result { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
        public string? Title { get; set; }
        public MessageType MessageType { get; set; }
        public NotifactionType? NotifactionType { get; set; }
    }
}
