using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Strategy Pattern

namespace SDP_Assignment.Jason
{
    public class WordConversionStrategy : IConversionStrategy
    {
        public string Convert(Document document)
        {
            return $"Converting {document.Title} to Word format";
        }
    }
}
