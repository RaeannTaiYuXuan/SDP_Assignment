using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.RAEANN
{
    public interface ISubject
    {
        void AttachObserver(NotifyObserver observer);
        void DetachObserver(NotifyObserver observer);
        void NotifyObservers(NotificationType type, string message, User excludeUser = null);
    }

}
