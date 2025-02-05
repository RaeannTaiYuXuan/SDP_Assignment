using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Factory Pattern

namespace SDP_Assignment.SHIYING
{
    public interface IDocumentFactory
    {
        IDocument CreateDocument(string title, string content, User owner);
    }
}
