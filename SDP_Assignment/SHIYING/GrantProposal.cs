using SDP_Assignment.RAEANN.COMPOSITE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.SHIYING
{
    public class GrantProposal : Document
    {
        public GrantProposal(string title, string content, User owner, IDocumentComponent header, IDocumentComponent footer)
            : base(title, header, footer, owner) // ✅ Pass header & footer
        {
            Content = content;
        }
    }

    // Raeann : i added composite pattern for header and footer , change the no of argument inside too 
}
