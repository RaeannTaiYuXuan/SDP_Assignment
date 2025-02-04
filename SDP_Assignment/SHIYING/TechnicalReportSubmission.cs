using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.SHIYING
{
    public class TechnicalReportSubmission : DocumentSubmissionTemplate
    {
        protected override void Validate()
        {
            Console.WriteLine("Validating Technical Report for accuracy and formatting.");
        }

        protected override void AssignApprover()
        {
            Console.WriteLine("Assigning approver from Research Committee.");
        }
    }
}