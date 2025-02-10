using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.SHIYING.DECORATOR
{
    public class SignatureDecorator : DocumentDecorator
    {
        public SignatureDecorator(Document document) : base(document)
        {
            if (document.HasDecorator("Signature"))
            {
                Console.WriteLine("Signature is already applied.");
                return;
            }

            document.AddDecorator("Signature");
            Console.WriteLine("Signature added.");
        }

        public override void Display()
        {
            base.Display();
            Console.WriteLine("[Signature Applied]");
        }
    }

}
