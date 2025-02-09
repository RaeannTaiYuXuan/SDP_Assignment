using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.SHIYING.DECORATOR
{
    public class WatermarkDecorator : DocumentDecorator
    {
        public WatermarkDecorator(Document document) : base(document)
        {
            if (document.HasDecorator("Watermark"))
            {
                Console.WriteLine("Watermark is already applied.");
                return;
            }

            document.AddDecorator("Watermark");
            Console.WriteLine("Watermark added.");
        }

        public override void Display()
        {
            base.Display();
            Console.WriteLine("[Watermark Applied]");
        }
    }

}
