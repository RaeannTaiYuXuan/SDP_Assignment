using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.RAEANN.COMPOSITE
{
    public class CompositeComponent : IDocumentComponent
    {
        //store multiple header components, allow individual headers and nested composite header to be stored, enables hierarchical header structure
        private List<IDocumentComponent> children = new List<IDocumentComponent>();

        public void Add(IDocumentComponent component) // to add new header , flexible because can add single header or multiple header inside
        {
            children.Add(component);
        }

        public void Remove(IDocumentComponent component) // remove header composite from composite
        {
            children.Remove(component);
        }

        public string Render()
        {
            StringBuilder sb = new StringBuilder(); // store combuine headers
            foreach (var child in children) // loop each child component  and ccalls render to it
            {
                sb.AppendLine(child.Render());
            }
            return sb.ToString(); // full string with all combined headers
        }
    }

}
