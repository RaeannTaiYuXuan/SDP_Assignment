using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.SHIYING
{
    public interface IDocument
    {
        string Title { get; }
        string Header { get; }
        string Content { get; set; }
        string Footer { get; }
        User Owner { get; }

        void Display();
    }
}
