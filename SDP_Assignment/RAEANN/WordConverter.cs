using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.RAEANN
{
    public class WordConverter : IConverter
    {
        public void Convert(Document document)
        {
            Console.WriteLine($"Document '{document.Title}' converted to Microsoft Word");
        }
    }

}
