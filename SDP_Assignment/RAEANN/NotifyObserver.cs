using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.RAEANN
{
    public interface NotifyObserver
    {
        void Update(string message);
        void StoreNotification(string message);
    }


}
