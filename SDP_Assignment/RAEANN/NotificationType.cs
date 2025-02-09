using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.RAEANN
{

//    Improves Readability – Clearly defines different types of notifications.
//✔ Prevents Invalid Values – Only allows predefined values (DocumentSubmitted, DocumentApproved, etc.).
//✔ Memory Efficient – Uses integer values behind the scenes, making it faster than a class.


    public enum NotificationType
    {
        DocumentSubmitted,
        DocumentApproved,
        DocumentRejected,
        DocumentPushedBack,
        CollaboratorAdded,
        DocumentEdited
    }
}
