using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDP_Assignment.RAEANN;

namespace SDP_Assignment
{
    public class User : INotifiable
    {
        private string name;

        public string Name { get; set; }

        public User(string name) 
        {
            Name = name;
        }

        public void Notify(string message)
        {
            Console.WriteLine($"Notification for {Name}: {message}");
        }
    }
}
