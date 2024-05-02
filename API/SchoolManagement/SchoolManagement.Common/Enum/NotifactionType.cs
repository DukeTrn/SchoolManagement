using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Common.Enum
{
    public enum NotifactionType
    {
        None = -1, // No need to notify
        Modal = 0, // Popup a modal, close when user operation
        Notification = 1, // Show at top right of screen, auto closed
    }
}
