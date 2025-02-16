using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.Jason.ITERATOR
{

    public class DocumentIterator : IIterator
    {
        private DocumentAggregate aggregate;
        private int currentIndex = 0;

        public DocumentIterator(DocumentAggregate aggregate)
        {
            this.aggregate = aggregate;
        }

        public bool HasNext()
        {
            return currentIndex < aggregate.Count;
        }

        public object Next()
        {
            if (!HasNext())
                throw new InvalidOperationException("No more documents.");
            return aggregate.GetDocument(currentIndex++);
        }
    }
}
