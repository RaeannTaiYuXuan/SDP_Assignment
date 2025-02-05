using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Factory Pattern

namespace SDP_Assignment.SHIYING
{
    public class GrantProposalFactory : IDocumentFactory
    {
        public IDocument CreateDocument(string title, string content, User owner)
        {
            return new Document(title, "GRANT PROPOSAL HEADER", "Grant Proposal Footer", owner)
            {
                Content = content
            };
        }
    }
}

