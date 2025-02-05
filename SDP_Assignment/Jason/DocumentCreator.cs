using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDP_Assignment.SHIYING;

// Factory Pattern

namespace SDP_Assignment.Jason
{
    public static class DocumentCreator
    {
        public static IDocumentFactory GetFactory(string type)
        {
            return type.ToLower() switch
            {
                "technical" => new TechnicalReportFactory(),
                "grant" => new GrantProposalFactory(),
                _ => throw new ArgumentException("Invalid document type")
            };
        }
    }
}
