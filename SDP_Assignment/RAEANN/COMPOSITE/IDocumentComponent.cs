using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.RAEANN.COMPOSITE
{
    public interface IDocumentComponent
    {
        // define common behavior for header and footer, llows us to treat headers and footers as interchangeable components.
        string Render();
    }
}
