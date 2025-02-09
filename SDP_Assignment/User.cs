using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDP_Assignment.RAEANN;

namespace SDP_Assignment
{
    public class User : NotifyObserver
    {
        public string Name { get; set; }
        private List<string> notificationHistory = new List<string>(); //Store notifications

        public User(string name)
        {
            Name = name;
        }

        public void Notify(NotificationType type, string message)
        {
            //Store notifications instead of printing them immediately
            StoreNotification(type, message);
        }

        public void StoreNotification(NotificationType type, string message)
        {
            string formattedMessage = $"[Notification] {type} - {message}";
            notificationHistory.Add(formattedMessage);
        }

        public void ShowNotifications()
        {
            Console.WriteLine($"\nNotifications for {Name}:");
            if (notificationHistory.Count == 0)
            {
                Console.WriteLine("No new notifications.");
            }
            else
            {
                foreach (string notification in notificationHistory)
                {
                    Console.WriteLine(notification);
                }
                notificationHistory.Clear(); // ✅ Clear notifications after viewing
            }
        }
    }
}
