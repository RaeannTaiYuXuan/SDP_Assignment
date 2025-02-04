using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Factory Pattern

namespace SDP_Assignment.Jason
{
    public class TechnicalReportFactory : IDocumentFactory
    {
        public Document CreateDocument(string title, string content, User owner)
        {
            return new Document(
                title,
                "TECHNICAL REPORT HEADER",
                "Technical Report Footer",
                owner
            )
            { Content = content };
        }
    }
}
