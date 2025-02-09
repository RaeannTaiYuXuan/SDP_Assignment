using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.RAEANN
{
    public interface NotifyObserver
    {
        void Notify(NotificationType type, string message);
        void StoreNotification(NotificationType type, string message);
    }


}
