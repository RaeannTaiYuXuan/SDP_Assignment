using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.SHIYING
{
    public class HeaderDecorator : DocumentDecorator
    {
        public HeaderDecorator(Document document) : base(document) { }

        public override void Display()
        {
            Console.WriteLine("[Header: " + Header + "]");
            base.Display();
        }
    }
}