using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.SHIYING
{
    public class GrantProposalSubmission : DocumentSubmissionTemplate
    {
        protected override void Validate()
        {
            Console.WriteLine("Checking Grant Proposal for budget constraints.");
        }

        protected override void AssignApprover()
        {
            Console.WriteLine("Assigning approver from Finance Department.");
        }
    }
}
