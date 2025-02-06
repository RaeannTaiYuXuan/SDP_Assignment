using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.SHIYING
{
    public class TechnicalReport : Document
    {
        public TechnicalReport(string title, string content, User owner)
            : base(title, "TECHNICAL REPORT HEADER", "Technical Report Footer", owner)
        {
            Content = content;
        }
    }
}
