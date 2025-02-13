using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.RAEANN
{
    public interface ISubject
    {
        void RegisterObserver(NotifyObserver observer);  
        void RemoveObserver(NotifyObserver observer);    
        void NotifyObservers(string message, User excludeUser = null);  
    }


}
