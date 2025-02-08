using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Strategy Pattern
namespace SDP_Assignment.Jason
{
    public interface ConvertStrategy
    {
        string Convert(Document document);
    }
}
