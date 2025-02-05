﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Factory Pattern

namespace SDP_Assignment.SHIYING
{
    public class TechnicalReportFactory : IDocumentFactory
    {
        public Document CreateDocument(string title, string content, User owner)
        {
            return new TechnicalReport(title, content, owner);
        }
    }
}

