using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Factory Pattern

namespace SDP_Assignment.Jason
{
    public interface IDocumentFactory
    {
        Document CreateDocument(string title, string content, User owner);
    }
}
