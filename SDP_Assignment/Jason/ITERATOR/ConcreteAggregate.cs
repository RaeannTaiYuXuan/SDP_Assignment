using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.Jason.ITERATOR
{
    public class DocumentAggregate : IAggregate
    {
        private List<Document> documents;

        public DocumentAggregate(List<Document> documents)
        {
            this.documents = documents;
        }

        public int Count
        {
            get { return documents.Count; }
        }

        public Document GetDocument(int index)
        {
            return documents[index];
        }

        public IIterator CreateIterator()
        {
            return new DocumentIterator(this);
        }
    }

}
