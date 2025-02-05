using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Factory Pattern

namespace SDP_Assignment.Jason
{
    public class GrantProposalFactory : IDocumentFactory
    {
        public Document CreateDocument(string title, string content, User owner)
        {
            return new Document(
                title,
                "GRANT PROPOSAL HEADER",
                "Grant Proposal Footer",
                owner
            )
            { Content = content };
        }
    }
}
