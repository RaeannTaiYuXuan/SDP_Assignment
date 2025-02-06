using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.SHIYING
{
    public class GrantProposal : Document
    {
        public GrantProposal(string title, string content, User owner)
            : base(title, "GRANT PROPOSAL HEADER", "Grant Proposal Footer", owner)
        {
            Content = content;
        }
    }
}
