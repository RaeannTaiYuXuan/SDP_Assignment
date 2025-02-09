using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.RAEANN.COMPOSITE
{
    public class HeaderComponent : IDocumentComponent
    {
        private string content;

        public HeaderComponent(string content)  // ✅ Ensure a constructor exists
        {
            this.content = content;
        }

        public string Render()
        {
            return content;
        }
    }

}
