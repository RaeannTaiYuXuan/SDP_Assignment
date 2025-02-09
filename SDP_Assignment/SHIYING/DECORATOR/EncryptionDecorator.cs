using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.SHIYING.DECORATOR
{
    public class EncryptionDecorator : DocumentDecorator
    {
        public EncryptionDecorator(Document document) : base(document)
        {
            if (document.HasDecorator("Encryption"))
            {
                Console.WriteLine("Encryption is already applied.");
                return;
            }

            document.AddDecorator("Encryption");
            Console.WriteLine("Document is now encrypted.");
        }

        public override void Display()
        {
            base.Display();
            Console.WriteLine("[Encrypted Document]");
        }
    }

}
