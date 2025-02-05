using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.SHIYING
{
    public class FooterDecorator : DocumentDecorator
    {
        public FooterDecorator(Document document) : base(document) { }

        public override void Display()
        {
            base.Display();
            Console.WriteLine("[Footer: " + Footer + "]");
        }
    }
}