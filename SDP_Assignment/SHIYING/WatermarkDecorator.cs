using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.SHIYING
{
    public class WatermarkDecorator : DocumentDecorator
    {
        private string watermark = "Confidential";

        public WatermarkDecorator(Document document) : base(document) { }

        public override void Display()
        {
            Console.WriteLine("[Watermark: " + watermark + "]");
            base.Display();
        }
    }
}