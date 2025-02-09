using SDP_Assignment.RAEANN.COMPOSITE;
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
        public Document CreateDocument(string title, string content, User owner, IDocumentComponent header, IDocumentComponent footer)
        {
            return new GrantProposal(title, content, owner, header, footer);
        }
    }

    // Raeann : i added composite pattern for header and footer , change the no of argument inside too 
}

